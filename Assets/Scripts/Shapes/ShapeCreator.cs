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
    public class ShapeCreator : MonoBehaviour
    {
        private GameManager _gameManager;
        private GameSettings _gameSettings;

        [SerializeField] private Transform _transformHolder;
        [SerializeField] private Transform _queueStartingPoint;
        [SerializeField] private Transform _queueEndPoint;

        [SerializeField] private ShapeFactory<ShapeType> _shapeFactory;
        [SerializeField] private List<ShapeManager> _shapes = new();

        private Vector3 _currentPosition;

        [Inject]
        private void InitializeDependencies(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
        }

        private void OnEnable()
        {
            MiniEventSystem.OnPlaceShape += OnPlaceShapeBehaviour;
            MiniEventSystem.OnCompleteSceneInit += InitializeShapeHolder;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= OnPlaceShapeBehaviour;
            MiniEventSystem.OnCompleteSceneInit -= InitializeShapeHolder;
        }

        private async void InitializeShapeHolder()
        {
            if (_shapes == null)
                return;

            _currentPosition = _queueStartingPoint.position;

            _shapes.Clear();

            await HandleCreateShapes();
            await RelocationShapes();

        }

        private async UniTask HandleCreateShapes()
        {
            for (int i = 0; i < 3; i++)
            {
                ShapeManager shape = _shapeFactory.Create(GetRandomShapeType(), _queueEndPoint.transform.position);

                _shapes.Add(shape);
            }

            await UniTask.DelayFrame(1);
        }

        private async UniTask RelocationShapes()
        {
            if (_shapes == null || _shapes.Count == 0)
                return;


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

        private void OnPlaceShapeBehaviour(ShapeManager shapeManager)
        {
            _shapes.Remove(shapeManager);


            if (_shapes.Count == 0)
            {
                InitializeShapeHolder();
                return;
            }

            ShapeHolderMatchCheck();
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
            int randomIndex = Random.Range(0, values.Length);
            return values[randomIndex];
        }
    }
}
