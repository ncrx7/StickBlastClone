using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using DataModel;
using Enums;
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
        [SerializeField] private ShapeFactory<ShapeType> _shapeFactory;

        [Inject]
        private void InitializeDependencies(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings, GameDataHandler gameDataHandler)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
            _gameDataHandler = gameDataHandler;
        }

        private void OnEnable()
        {
            SetShapeHolderService();

            MiniEventSystem.OnPlaceShape += _shapeHolderCreateService.OnPlaceCallBack;
            MiniEventSystem.OnCompleteSceneInit += Setup;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= _shapeHolderCreateService.OnPlaceCallBack;
            MiniEventSystem.OnCompleteSceneInit -= Setup;
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
                    _shapeHolderCreateService = new ShapeHolderQueue(_shapeFactory, _gameManager, _gameSettings);
                    break;
                case ShapeHolderType.UnOrdered:
                    _shapeHolderCreateService = new ShapeHolderUnordered(_shapeFactory, _gameManager, _gameSettings);
                    break;
            }

            await UniTask.Delay(50);
            MiniEventSystem.OnShapeHolderServiceSetted?.Invoke(_shapeHolderType);
        }
    }
}
