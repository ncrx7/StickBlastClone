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
            _shapeTypeblackList.Clear();

            for (int i = 0; i < 3; i++)
            {
                await CreateMatchingShapeByBoard();
            }

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
                //ShapeManager shape = _shapeFactory.Create(GetRandomShapeType(), QueueEndPoint.transform.position);
                ShapeWrapper<ShapeType> candidateShapeWrapper = _gameSettings.ShapeData[UnityEngine.Random.Range(0, _gameSettings.ShapeData.Count)];

                if (_shapeTypeblackList.Contains(candidateShapeWrapper.Type))
                {
                    //Debug.Log("type of " + candidateShapeWrapper.Type + " is in the black list.");
                    continue;
                }

                var response = await PathChecker.EmptyDirectionPathOnBoardChecker(_nodeGridBoardManager, candidateShapeWrapper.ShapePrefab);

                if (response.IsThereEmptySlot)
                {
                    ShapeManager shape = _shapeFactory.Create(candidateShapeWrapper.Type, QueueEndPoint.transform.position);
                    _shapes.Add(shape);
                    choosingCorrected = true;

                    if (!response.IsSlotCountUpperThanOne)
                    {
                        //Debug.Log("shape : " + shape.name + " has only one match on board");
                        _shapeTypeblackList.Add(candidateShapeWrapper.Type);
                    }
                }
            }
        }

        public async void OnPlaceCallBack(ShapeManager shapeManager)
        {
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
