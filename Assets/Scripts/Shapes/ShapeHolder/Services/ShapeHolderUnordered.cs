using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModel;
using DG.Tweening;
using Enums;
using UnityEngine;
using Zenject;

namespace Shapes
{
    public class ShapeHolderUnordered : IShapeHolderCreateService
    {
        private GameManager _gameManager;
        private GameSettings _gameSettings;

   
        private ShapeFactory<ShapeType> _shapeFactory;
        private List<ShapeManager> _shapes = new();

        private Vector3 _currentPosition;

        public Transform QueueStartingPoint { get => _queueStartPoint; set => _queueStartPoint = value; }
        public Transform QueueEndPoint { get => _queueEndPoint; set => _queueEndPoint = value; }

        private Transform _queueStartPoint;
        private Transform _queueEndPoint;

        public ShapeHolderUnordered(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
        }

        private void ShapeHolderMatchCheck()
        {
            bool anyMatchExists = false;

            foreach (var shape in _shapes)
            {
                if (shape.CheckRelativeMatchExist())
                    anyMatchExists = true;
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


        public async void OnPlaceCallBack(ShapeManager shapeManager)
        {
            _shapes.Remove(shapeManager);


            if (_shapes.Count == 0)
            {
                await HandleCreateShapes();
                await RelocationShapes();

                return;
            }

            ShapeHolderMatchCheck();
        }

        public async UniTask HandleCreateShapes()
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeManager shape = _shapeFactory.Create(GetRandomShapeType(), QueueEndPoint.transform.position);

                _shapes.Add(shape);
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

            ShapeHolderMatchCheck();
        }

    }
}
