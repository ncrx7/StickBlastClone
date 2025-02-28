using System.Collections;
using System.Collections.Generic;
using MyUtils.Base;
using NodeGridSystem.Models;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;
using Enums;

namespace NodeGridSystem.Controllers
{
    public class NodeGridBoardManager : SingletonBehavior<NodeGridBoardManager>
    {
        [Header("Node Grid Settings")]
        [SerializeField] private int _width = 6;
        [SerializeField] private int _height = 6;
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private Vector3 _originPosition = Vector3.zero;
        [SerializeField] private bool _debug = true;

        private NodeGridSystem2D<GridNodeObject<NodeManager>> _nodeGrid;
        private NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>> _middleObjectGrid;
        [SerializeField] private EdgeManager edgePrefab;

        [SerializeField] private MiddleFillAreaManager _middleFillAreaPrefab;

        private void Start()
        {
            InitializeBoard();
        }

        private async void InitializeBoard()
        {
            _nodeGrid = NodeGridSystem2D<GridNodeObject<NodeManager>>.VerticalGrid(_width, _height, _cellSize, _originPosition, _debug);
            _middleObjectGrid = NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>.VerticalGrid(_width - 1, _height - 1, _cellSize, _originPosition, _debug);

            await InitNodes();
            await InitNeigbours();
            await InitMiddleArea();
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

        public void CheckMidCellFullnessOnBoard()
        {

            /*             var rightGridObject = _middleObjectGrid.GetValue(x, y - 1);
                        var leftGridObject = _middleObjectGrid.GetValue(x - 1, y - 1);
                        var topGridObject = _middleObjectGrid.GetValue(x, y);
                        var downGridObject = _middleObjectGrid.GetValue(x, y - 1);

                        List<MiddleFillAreaManager> midCells = new()
                        {
                            rightGridObject?.GetValue(),
                            leftGridObject?.GetValue(),
                            topGridObject?.GetValue(),
                            downGridObject?.GetValue()
                        }; */

            /* foreach (var midCell in midCells)
            {
                bool checkResponse = midCell.CheckEdges();

                if (checkResponse)
                {
                    midCell.GetSpriteRenderer.enabled = true;
                    midCell.transform.localScale = new Vector3(7, 7, 7);
                }
            } */

            for (int x = 0; x < _width - 1; x++)
            {
                for (int y = 0; y < _height - 1; y++)
                {
                    var midCellGridObject = _middleObjectGrid.GetValue(x, y);
                    MiddleFillAreaManager midCell = midCellGridObject.GetValue();

                    bool checkResponse = midCell.CheckEdges();

                    if (checkResponse)
                    {
                        if(!midCell.IsFilled) midCell.OnAllEdgeFull();

                        ColumnCheckerOnBoard();
                        RowCheckerOnBoard();
                    }
                }
            }

            /* MiddleFillAreaManager rightMidCell = rightGridObject.GetValue();
            MiddleFillAreaManager leftMidCell = leftGridObject.GetValue();
            MiddleFillAreaManager topMidCell = topGridObject.GetValue();
            MiddleFillAreaManager downMidCell = downGridObject.GetValue(); */
        }

        private async void RowCheckerOnBoard()
        {
            List<MiddleFillAreaManager> midCells = new();

            for (int y = 0; y < _height - 1; y++)
            {
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
                    foreach (var midCell in midCells)
                    {
                        midCell.GetSpriteRenderer.enabled = false;
                        midCell.IsFilled = false;
                        midCell.ResetEdges();
                        MiniEventSystem.PlaySoundClip?.Invoke(SoundType.QueueCellsExplosion);
                        await UniTask.Delay(50);
                    }
                }
            }
        }

        private async void ColumnCheckerOnBoard()
        {
            List<MiddleFillAreaManager> midCells = new();

            for (int x = 0; x < _width - 1; x++)
            {
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
                    foreach (var midCell in midCells)
                    {
                        midCell.GetSpriteRenderer.enabled = false;
                        midCell.IsFilled = false;
                        midCell.ResetEdges();
                        MiniEventSystem.PlaySoundClip?.Invoke(SoundType.QueueCellsExplosion);
                        await UniTask.Delay(50);
                    }
                }
            }
        }

        public float GetCellSize => _cellSize;
    }
}