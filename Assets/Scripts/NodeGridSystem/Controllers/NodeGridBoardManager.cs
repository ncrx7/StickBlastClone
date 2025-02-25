using System.Collections;
using System.Collections.Generic;
using MyUtils.Base;
using NodeGridSystem.Models;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;

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

        private void Start()
        {
            InitializeBoard();
        }

        private async void InitializeBoard()
        {
            _nodeGrid = NodeGridSystem2D<GridNodeObject<NodeManager>>.VerticalGrid(_width, _height, _cellSize, _originPosition, _debug);

            await InitNodes();
            await InitNeigbours();
        }

        private async UniTask InitNodes()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    //Match3Events.CreateGemObject?.Invoke(x, y, _grid, GemTypes, _gemPoolId);
                    MiniEventSystem.OnCreateNode?.Invoke(x, y, _nodeGrid, 1);
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
                    var gridObject = _nodeGrid.GetValue(x, y);
                    gridObject.InitNeighbourGridObjects();
                }
            }

            await UniTask.DelayFrame(1);
        }
    }
}