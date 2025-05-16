using System.Collections;
using System.Collections.Generic;
using UnityUtils.BaseClasses;
using NodeGridSystem.Models;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;
using Enums;
using Zenject;
using Level;
using DataModel;
using Data.Controllers;
using System.Linq;

namespace NodeGridSystem.Controllers
{
    public class NodeGridBoardManager : MonoBehaviour
    {
        private GameSettings _gameSettings;
        private GameDataHandler _gameDataHandler;

        private NodeGridSystem2D<GridNodeObject<NodeManager>> _nodeGrid;
        private NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> _middleObjectGrid;

        private Camera _mainCam;
        private GameManager _gameManager;
        private CameraManager _cameraManager;
        private ComboManager _comboManager;

        private int _width;
        private int _height;

        [SerializeField] private List<EdgeManager> _allEdges;

        public float AutomaticBoardCellSize { get; private set; }
        private Vector3 AutomaticOffset = Vector3.zero;

        public bool CheckingMidCells;

        [Inject]
        private void InitializeDependencies(CameraManager cameraManager, GameManager gameManager, ComboManager comboManager, GameSettings gameSettings, GameDataHandler gameDataHandler, Camera mainCam)
        {
            _cameraManager = cameraManager;
            _gameManager = gameManager;
            _comboManager = comboManager;
            _gameSettings = gameSettings;
            _gameDataHandler = gameDataHandler;
            _mainCam = mainCam;
        }

        private async void Start()
        {
            _width = _gameDataHandler.GetGameDataObjectReference().settings.GridWidth;
            _height = _gameDataHandler.GetGameDataObjectReference().settings.GridHeight;

            await CalculateDimensions();
            await InitializeBoard();
        }

        private async UniTask CalculateDimensions()
        {
            MiniEventSystem.ActivateLoadingUI?.Invoke();
            _gameManager.IsGamePaused = true;

            float screenHeight = 2f * _mainCam.orthographicSize;
            float screenWidth = screenHeight * _mainCam.aspect;

            float maxGridWidth = screenWidth * _gameSettings.XScreenUsageRate;
            float maxGridHeight = screenHeight * _gameSettings.YScreenUsageRate;

            float availableCellWidth = (maxGridWidth) / _width;
            float availableCellHeight = (maxGridHeight) / _height;

            float cellSize = Mathf.Min(availableCellWidth, availableCellHeight);

            AutomaticBoardCellSize = cellSize;

            await UniTask.Delay(100);

            AutomaticOffset.x = -(_width / 2f * AutomaticBoardCellSize);
            AutomaticOffset.y = -(_height / 2f * AutomaticBoardCellSize);

            AutomaticOffset += _gameSettings.OffsetFromCenter;

            MiniEventSystem.OnCompleteGridBoardDimensionCalculating?.Invoke();
        }

        private async UniTask InitializeBoard()
        {
            _nodeGrid = NodeGridSystem2D<GridNodeObject<NodeManager>>.VerticalGrid(_width, _height, AutomaticBoardCellSize, AutomaticOffset, _gameSettings.Debug);
            _middleObjectGrid = NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>.VerticalGrid(_width - 1, _height - 1, AutomaticBoardCellSize, AutomaticOffset, _gameSettings.Debug);

            await InitNodes();
            await InitNeigbours();
            await InitMiddleArea();
            await UniTask.Delay(1000);

            _gameManager.IsGamePaused = false;
            MiniEventSystem.DeactivateLoadingUI?.Invoke();
        }

        private async UniTask InitNodes()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.NodeGrid, x, y, _nodeGrid, _middleObjectGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
        }

        private async UniTask InitNeigbours()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var gridNodeObject = _nodeGrid.GetValue(x, y);
                    gridNodeObject.InitNeighbourGridObjects();

                    //await InitEdges(x, y, gridNodeObject);
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.Edge, x, y, _nodeGrid, _middleObjectGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
            //TODO: Board has been inited, write here a flag to disappear loading panel (use wait until)
        }

        private async UniTask InitMiddleArea()
        {
            for (int x = 0; x < _width - 1; x++)
            {
                for (int y = 0; y < _height - 1; y++)
                {
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.MidCell, x, y, _nodeGrid, _middleObjectGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
        }

        public async void CheckMidCellFullnessOnBoard(List<EdgeManager> edgeManagers)
        {
            CheckingMidCells = true;

            bool MatchExist = false;

            foreach (var edge in edgeManagers.ToList()) //await işlemleri eklendiginden dolayı ve islemler sırasında bu liste degistirilebileceğinden hata oluşmaması adına kopyasını olusturdum
            {
                foreach (var midCell in edge.GetMidCells)
                {
                    bool checkResponse = midCell.CheckEdges();

                    if (checkResponse)
                    {
                        if (!midCell.IsFilled)
                        {
                            MatchExist = true;

                            midCell.OnAllEdgeFull();

                            await ColumnCheckerOnBoard(midCell.OnGridNodeObject.GetX);

                            await RowCheckerOnBoard(midCell.OnGridNodeObject.GetY);
                        }
                    }
                }
            }

            CheckingMidCells = false;

            if (!MatchExist)
            {
                _comboManager.ResetCombo();
            }
        }

        private async UniTask RowCheckerOnBoard(int y)
        {
            List<MiddleFillAreaManager> midCells = new();

            midCells.Clear();
            bool rowCanDestroy = true;

            for (int x = 0; x < _width - 1; x++)
            {
                var midCellGridObject = _middleObjectGrid.GetValue(x, y);
                MiddleFillAreaManager midCell = midCellGridObject.GetValue();

                if (!midCell.IsFilled)
                {
                    rowCanDestroy = false;
                    break;
                }

                midCells.Add(midCell);
            }

            if (rowCanDestroy)
            {
                _cameraManager.ZoomInAndOut(5.2f, 0.45f, 0.2f, 8);
                _comboManager.ResetCombo();

                foreach (var midCell in midCells)
                {
                    midCell.GetSpriteRenderer.enabled = false;
                    midCell.IsFilled = false;
                    midCell.ResetEdges();

                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.QueueCellsExplosion);
                    MiniEventSystem.PlayVfx?.Invoke(midCell.transform.position, VfxType.CellDestroy);
                    MiniEventSystem.PlayVfx?.Invoke(midCell.transform.position, VfxType.CellSmoke);
                    MiniEventSystem.IncreaseScore?.Invoke(_gameManager.GetScore, _gameManager.GetScore + _gameManager.GetScoreIncreaseAmountPerCellDestroy);

                    await UniTask.Delay(50);
                }
            }

        }

        private async UniTask ColumnCheckerOnBoard(int x)
        {
            List<MiddleFillAreaManager> midCells = new();

            midCells.Clear();
            bool columnCanDestroy = true;

            for (int y = 0; y < _height - 1; y++)
            {
                var midCellGridObject = _middleObjectGrid.GetValue(x, y);
                MiddleFillAreaManager midCell = midCellGridObject.GetValue();

                if (!midCell.IsFilled)
                {
                    columnCanDestroy = false;
                    break;
                }

                midCells.Add(midCell);
            }

            if (columnCanDestroy)
            {
                _cameraManager.ZoomInAndOut(5.2f, 0.45f, 0.2f, 8);
                _comboManager.ResetCombo();

                foreach (var midCell in midCells)
                {
                    midCell.GetSpriteRenderer.enabled = false;
                    midCell.IsFilled = false;
                    midCell.ResetEdges();

                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.QueueCellsExplosion);

                    MiniEventSystem.PlayVfx?.Invoke(midCell.transform.position, VfxType.CellDestroy);
                    MiniEventSystem.PlayVfx?.Invoke(midCell.transform.position, VfxType.CellSmoke);
                    MiniEventSystem.IncreaseScore?.Invoke(_gameManager.GetScore, _gameManager.GetScore + _gameManager.GetScoreIncreaseAmountPerCellDestroy);

                    await UniTask.Delay(50);
                }
            }

        }

        public bool AllEdgeIsFull()
        {
            return GetAllEdgesOnBoard.All(edge => !edge.IsEmpty);
        }

        public int GetWidth => _width;
        public int GetHeight => _height;
        public NodeGridSystem2D<GridNodeObject<NodeManager>> GetNodeGridSystem2D => _nodeGrid;
        public List<EdgeManager> GetAllEdgesOnBoard => _allEdges;

    }
}