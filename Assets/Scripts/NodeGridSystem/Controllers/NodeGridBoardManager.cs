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
            _middleObjectGrid = NodeGridSystem2D<GridNodeObject<MiddleFillAreaManager>>.VerticalGrid(_width - 1, _height - 1, _cellSize, _originPosition, true);

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
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.NodeGrid, x, y, _nodeGrid, 1);
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
                    MiniEventSystem.OnCreateEntity?.Invoke(EntityType.Edge, x, y, _nodeGrid, 1);
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
                    MiddleFillAreaManager middleArea = Instantiate(_middleFillAreaPrefab, _middleObjectGrid.GetWorldPositionCenter(x, y), Quaternion.identity, transform);

                    middleArea.transform.position = _middleObjectGrid.GetWorldPositionCenter(x, y) + new Vector3(_cellSize / 2, _cellSize / 2 + 0);
                    middleArea.transform.SetParent(transform);

                    var gridObject = new GridNodeObject<MiddleFillAreaManager>(_middleObjectGrid, x, y);
                    gridObject.InitNeighbourGridObjects();

                    gridObject.SetValue(middleArea);
                    _middleObjectGrid.SetValue(x, y, gridObject);

                    middleArea.SetGridObjectOnMiddleArea(gridObject);

                    middleArea.Setup(x, y, _nodeGrid);

                    //MiniEventSystem.OnCreateEntity?.Invoke(EntityType.NodeGrid, x, y, _nodeGrid, 1);
                }
            }

            await UniTask.DelayFrame(1);
        }
    }
}