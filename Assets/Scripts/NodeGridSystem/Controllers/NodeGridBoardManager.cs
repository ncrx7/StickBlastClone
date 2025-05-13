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

namespace NodeGridSystem.Controllers
{
    public class NodeGridBoardManager : MonoBehaviour
    {
        private GameSettings _gameSettings;

        private NodeGridSystem2D<GridNodeObject<NodeManager>> _nodeGrid;
        private NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> _middleObjectGrid;

        private GameManager _gameManager;
        private CameraManager _cameraManager;
        private ComboManager _comboManager;


        [Inject]
        private void InitializeDependencies(CameraManager cameraManager, GameManager gameManager, ComboManager comboManager, GameSettings gameSettings)
        {
            _cameraManager = cameraManager;
            _gameManager = gameManager;
            _comboManager = comboManager;
            _gameSettings = gameSettings;
        }

        private void Start()
        {
            InitializeBoard();
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.L))
            {
                MiniEventSystem.IncreaseScore?.Invoke(_gameManager.GetScore, _gameManager.GetScore + _gameManager.GetScoreIncreaseAmountPerCellDestroy);
            }
        }

        private async void InitializeBoard()
        {
            _nodeGrid = NodeGridSystem2D<GridNodeObject<NodeManager>>.VerticalGrid(_gameSettings.Width, _gameSettings.height, _gameSettings.CellSize, _gameSettings.OriginPosition, _gameSettings.Debug);
            _middleObjectGrid = NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>.VerticalGrid(_gameSettings.Width - 1, _gameSettings.height - 1, _gameSettings.CellSize, _gameSettings.OriginPosition, _gameSettings.Debug);

            MiniEventSystem.ActivateLoadingUI?.Invoke();
            _gameManager.IsGamePaused = true;

            await InitNodes();
            await InitNeigbours();
            await InitMiddleArea();
            await UniTask.Delay(1000);

            _gameManager.IsGamePaused = false;
            MiniEventSystem.DeactivateLoadingUI?.Invoke();
        }

        private async UniTask InitNodes()
        {
            for (int x = 0; x < _gameSettings.Width; x++)
            {
                for (int y = 0; y < _gameSettings.height; y++)
                {
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.NodeGrid, x, y, _nodeGrid, _middleObjectGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
        }

        private async UniTask InitNeigbours()
        {
            for (int x = 0; x < _gameSettings.Width; x++)
            {
                for (int y = 0; y < _gameSettings.height; y++)
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
            for (int x = 0; x < _gameSettings.Width - 1; x++)
            {
                for (int y = 0; y < _gameSettings.height - 1; y++)
                {
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.MidCell, x, y, _nodeGrid, _middleObjectGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
        }

        public void CheckMidCellFullnessOnBoard(List<EdgeManager> edgeManagers)
        {
            bool MatchExist = false;

            foreach (var edge in edgeManagers)
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

                            ColumnCheckerOnBoard(midCell.OnGridNodeObject.GetX);

                            RowCheckerOnBoard(midCell.OnGridNodeObject.GetY);
                        }
                    }
                }
            }

            if (!MatchExist)
            {
                _comboManager.ResetCombo();
            }
        }

        private async void RowCheckerOnBoard(int y)
        {
            List<MiddleFillAreaManager> midCells = new();

            midCells.Clear();
            bool rowCanDestroy = true;

            for (int x = 0; x < _gameSettings.Width - 1; x++)
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

        private async void ColumnCheckerOnBoard(int x)
        {
            List<MiddleFillAreaManager> midCells = new();

            midCells.Clear();
            bool columnCanDestroy = true;

            for (int y = 0; y < _gameSettings.height - 1; y++)
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

        public float GetCellSize => _gameSettings.CellSize;
        public int GetWidth => _gameSettings.Width;
        public int GetHeight => _gameSettings.height;
        public NodeGridSystem2D<GridNodeObject<NodeManager>> GetNodeGridSystem2D => _nodeGrid;

    }
}