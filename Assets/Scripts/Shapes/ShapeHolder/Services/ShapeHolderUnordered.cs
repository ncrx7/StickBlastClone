using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModel;
using DG.Tweening;
using EntitiesData.Shapes;
using Enums;
using NodeGridSystem.Controllers;
using UnityEngine;
using Zenject;

namespace Shapes
{
    public class ShapeHolderUnordered : IShapeHolderCreateService
    {
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        private NodeGridBoardManager _nodeGridBoardManager;


        private ShapeFactory<ShapeType> _shapeFactory;
        private List<ShapeManager> _shapes = new();

        private Vector3 _currentPosition;

        public Transform QueueStartingPoint { get => _queueStartPoint; set => _queueStartPoint = value; }
        public Transform QueueEndPoint { get => _queueEndPoint; set => _queueEndPoint = value; }

        private Transform _queueStartPoint;
        private Transform _queueEndPoint;

        private List<ShapeType> _shapeTypeblackList = new();
        private List<EdgeManager> _tempEdges = new();
        ShapeType lastShapeType;

        public ShapeHolderUnordered(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings, NodeGridBoardManager nodeGridBoardManager)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
            _nodeGridBoardManager = nodeGridBoardManager;
        }

        public async void InitializeShapeHolder(Transform startPoint, Transform endPoint)
        {
            if (_shapes == null)
                return;

            QueueStartingPoint = startPoint;
            QueueEndPoint = endPoint;

            _currentPosition = startPoint.position;

            _shapes.Clear();

            await HandleCreateShapes();
            await RelocationShapes();
        }

        public async UniTask HandleCreateShapes()
        {
            for (int i = 0; i < 3; i++)
            {
                await CreateMatchingShapeByBoard();
            }
            await UniTask.Delay(20);

            foreach (var item in _tempEdges)
            {
                item.IsEmpty = true;
            }

            _tempEdges.Clear();

            await UniTask.DelayFrame(1);
        }

        public async UniTask RelocationShapes()
        {
            if (_shapes == null || _shapes.Count == 0)
                return;

            _currentPosition = QueueStartingPoint.position;

            foreach (var shape in _shapes)
            {
                shape.transform.DOMove(_currentPosition, _gameSettings.AnimationTime).OnComplete(() =>
                {
                    shape.SetCanMoveFlag(true);
                }
                );

                _currentPosition.x -= _gameSettings.Margin;

                await UniTask.Delay(50);
            }
            
            ShapeHolderItemsMatchCheck();
        }

        private async UniTask CreateMatchingShapeByBoard()
        {
            bool choosingCorrected = false;

            while (!choosingCorrected)
            {
                ShapeWrapper<ShapeType> candidateShapeWrapper = _gameSettings.ShapeData[UnityEngine.Random.Range(0, _gameSettings.ShapeData.Count)];

                var response = await PathChecker.EmptyDirectionPathOnBoardChecker(_nodeGridBoardManager, candidateShapeWrapper.ShapePrefab, _tempEdges);

                if (response.IsThereEmptySlot)
                {
                    ShapeManager shape = _shapeFactory.Create(candidateShapeWrapper.Type, QueueEndPoint.transform.position);
                    lastShapeType = candidateShapeWrapper.Type;
                    _shapes.Add(shape);
                    choosingCorrected = true;
                    return;
                }

                if (_nodeGridBoardManager.AllEdgeIsFull)
                    break;
            }

            ShapeManager shapeLast = _shapeFactory.Create(lastShapeType, QueueEndPoint.transform.position);
            _shapes.Add(shapeLast);
            //Debug.Log("last shape created outside of while!!!!!!!!!!!!!!!!");
        }

        public async void OnPlaceCallBack(ShapeManager shapeManager)
        {
            await UniTask.WaitUntil(() => !_nodeGridBoardManager.CheckingMidCells); //Board Kontrollerinin bitmesini bekliyoruz

            _shapes.Remove(shapeManager);


            if (_shapes.Count == 0)
            {
                await HandleCreateShapes();
                await RelocationShapes();

                return;
            }

            ShapeHolderItemsMatchCheck();
        }

        private async void ShapeHolderItemsMatchCheck()
        {
            await UniTask.WaitUntil(() => !_nodeGridBoardManager.CheckingMidCells);
            
            bool anyMatchExists = false;

            foreach (var shape in _shapes)
            {
                var response = await PathChecker.EmptyDirectionPathOnBoardChecker(_nodeGridBoardManager, shape);

                if (response.IsThereEmptySlot)
                {
                    anyMatchExists = true;
                }
            }


            if (!anyMatchExists && !_gameManager.IsGamePaused)
            {
                MiniEventSystem.OnEndGame?.Invoke(0);
            }
        }

        public static ShapeType GetRandomShapeType()
        {
            ShapeType[] values = (ShapeType[])System.Enum.GetValues(typeof(ShapeType));
            int randomIndex = UnityEngine.Random.Range(0, values.Length);
            return values[randomIndex];
        }
    }
}
