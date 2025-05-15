using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using DataModel;
using Enums;
using NodeGridSystem.Controllers;
using NodeGridSystem.Controllers.EntityScalers;
using UnityEngine;
using Zenject;

namespace Shapes
{
    public class ShapeHolderCreator : MonoBehaviour
    {
        public enum ShapeHolderType {UnOrdered, Queue}

        private IShapeHolderCreateService _shapeHolderCreateService;
        [SerializeField] private ShapeHolderType _shapeHolderType;

        [SerializeField] private Transform _transformHolder;
        [SerializeField] private Transform _queueStartingPoint;
        [SerializeField] private Transform _queueEndPoint;


        private GameDataHandler _gameDataHandler;
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        private EntityScaler _entityScaler;
        private NodeGridBoardManager _nodeGridBoardManager;
        [SerializeField] private ShapeFactory<ShapeType> _shapeFactory;

        [Inject]
        private void InitializeDependencies(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings, GameDataHandler gameDataHandler,
                         EntityScaler entityScaler, NodeGridBoardManager nodeGridBoardManager)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
            _gameDataHandler = gameDataHandler;
            _entityScaler = entityScaler;
            _nodeGridBoardManager = nodeGridBoardManager;
        }

        private void OnEnable()
        {
            SetShapeHolderService();

            MiniEventSystem.OnPlaceShape += _shapeHolderCreateService.OnPlaceCallBack;
            MiniEventSystem.OnCompleteSceneInit += Setup;

            MiniEventSystem.OnCompleteGridBoardDimensionCalculating += HandleShapeScale;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= _shapeHolderCreateService.OnPlaceCallBack;
            MiniEventSystem.OnCompleteSceneInit -= Setup;

            MiniEventSystem.OnCompleteGridBoardDimensionCalculating -= HandleShapeScale;
        }

        private void HandleShapeScale()
        {
            _entityScaler.CalculateShapeScaleFactor(_gameSettings.ShapeData[0].ShapePrefab, _nodeGridBoardManager);
        }

        private void Setup()
        {
            _shapeHolderCreateService.InitializeShapeHolder(_queueStartingPoint, _queueEndPoint);
        }

        private async void SetShapeHolderService()
        {
            _shapeHolderType = _gameDataHandler.GetGameDataObjectReference().settings.ShapeHolderType;

            switch (_shapeHolderType)
            {
                case ShapeHolderType.Queue:
                    _shapeHolderCreateService = new ShapeHolderQueue(_shapeFactory, _gameManager, _gameSettings, _nodeGridBoardManager);
                    break;
                case ShapeHolderType.UnOrdered:
                    _shapeHolderCreateService = new ShapeHolderUnordered(_shapeFactory, _gameManager, _gameSettings, _nodeGridBoardManager);
                    break;
            }

            await UniTask.Delay(50);
            MiniEventSystem.OnShapeHolderServiceSetted?.Invoke(_shapeHolderType);
        }
    }
}
