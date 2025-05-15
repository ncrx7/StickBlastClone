using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityUtils.BaseClasses;
using Enums;
using Zenject;
using DataModel;
using NodeGridSystem.Controllers;


namespace Shapes
{
    public class ShapeHolderQueue : IShapeHolderCreateService
    {
        private GameManager _gameManager;
        private GameSettings _gameSettings;
        private NodeGridBoardManager _nodeGridBoardManager;

        [SerializeField] private Queue<ShapeManager> _shapeQueue = new();
        [SerializeField] private ShapeFactory<ShapeType> _shapeFactory;

        private Vector3 _currentPosition;

        public Transform QueueStartingPoint { get => _queueStartPoint; set => _queueStartPoint = value; }
        public Transform QueueEndPoint { get => _queueEndPoint; set => _queueEndPoint = value; }

        private Transform _queueStartPoint;
        private Transform _queueEndPoint;

        public ShapeHolderQueue(ShapeFactory<ShapeType> shapeFactory, GameManager gameManager, GameSettings gameSettings, NodeGridBoardManager nodeGridBoardManager)
        {
            _shapeFactory = shapeFactory;
            _gameManager = gameManager;
            _gameSettings = gameSettings;
            _nodeGridBoardManager = nodeGridBoardManager;
        }
        public async void InitializeShapeHolder(Transform startpoint, Transform endPoint)
        {
            QueueStartingPoint = startpoint;
            QueueEndPoint = endPoint;

            _currentPosition = startpoint.position;

            await HandleCreateShapes();
            await RelocationShapes();
        }

        public async UniTask HandleCreateShapes()
        {
            for (int i = 0; i < 6; i++)
            {
                //ShapeManager shape = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, _transformHolder);
                ShapeManager shape = _shapeFactory.Create(GetRandomShapeType(), _queueEndPoint.transform.position);
                _shapeQueue.Enqueue(shape);
            }

            await UniTask.DelayFrame(1);
        }

        public async UniTask RelocationShapes()
        {
            _currentPosition = QueueStartingPoint.position;
            int index = 0;

            foreach (ShapeManager shape in _shapeQueue)
            {
                shape.transform.DOMove(_currentPosition, _gameSettings.AnimationTime).OnComplete(async () =>
                {
                    if (index == 0)
                    {
                        
                        var response = await PathChecker.EmptyDirectionPathOnBoardChecker(_nodeGridBoardManager, shape);

                        if (!response.IsThereEmptySlot)
                            MiniEventSystem.OnEndGame?.Invoke(0);

                        shape.SetCanMoveFlag(true);
                    }

                    index++;
                }
                );

                _currentPosition.x -= _gameSettings.Margin;

                await UniTask.Delay(50);
            }
        }

        public async void OnPlaceCallBack(ShapeManager shapeManager)
        {
            _shapeQueue.Dequeue();

            //ShapeManager shapeSpawned = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, _transformHolder);
            ShapeManager shapeSpawned = _shapeFactory.Create(GetRandomShapeType(), _queueEndPoint.transform.position);

            _shapeQueue.Enqueue(shapeSpawned);

            await RelocationShapes();
        }

        /*         private void OnEnable()
                {
                    MiniEventSystem.OnPlaceShape += HandleDequeue;
                    MiniEventSystem.OnCompleteSceneInit += StartQueue;
                }

                private void OnDisable()
                {
                    MiniEventSystem.OnPlaceShape -= HandleDequeue;
                    MiniEventSystem.OnCompleteSceneInit -= StartQueue;
                }

                private async void StartQueue()
                {
                    _currentPosition = _queueStartingPoint.position;

                    await CreateQueue();
                    await RelocationShapes();
                } */
        /* #if UNITY_EDITOR
                private void Update()
                {
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        RelocationShapes().Forget();
                    }

                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        _shapeFactory.Create(ShapeType.Snake, new Vector3(0, 0, 0));
                    }
                }
        #endif */

        /*         private async UniTask CreateQueue()
                {
                    for (int i = 0; i < 6; i++)
                    {
                        //ShapeManager shape = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, _transformHolder);
                        ShapeManager shape = _shapeFactory.Create(GetRandomShapeType(), _queueEndPoint.transform.position);
                        _shapeQueue.Enqueue(shape);
                    }

                    await UniTask.DelayFrame(1);
                }

                private async UniTask RelocationShapes()
                {
                    _currentPosition = _queueStartingPoint.position;
                    int index = 0;

                    foreach (ShapeManager shape in _shapeQueue)
                    {
                        shape.transform.DOMove(_currentPosition, _gameSettings.AnimationTime).OnComplete(() =>
                        {
                            if (index == 0)
                            {
                                if (!shape.CheckRelativeMatchExist() && !_gameManager.IsGamePaused)
                                    MiniEventSystem.OnEndGame?.Invoke(0);

                                shape.SetCanMoveFlag(true);
                            }

                            index++;
                        }
                        );

                        _currentPosition.x -= _gameSettings.Margin;

                        await UniTask.Delay(50);
                    }
                } */

        /* private void HandleDequeue(ShapeManager shapeManager)
        {
            DequeueShape().Forget();
        } */

        /*         private async UniTask DequeueShape()
                {
                    _shapeQueue.Dequeue();

                    //ShapeManager shapeSpawned = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, _transformHolder);
                    ShapeManager shapeSpawned = _shapeFactory.Create(GetRandomShapeType(), _queueEndPoint.transform.position);

                    _shapeQueue.Enqueue(shapeSpawned);

                    await RelocationShapes();

                } */

        public static ShapeType GetRandomShapeType()
        {
            ShapeType[] values = (ShapeType[])System.Enum.GetValues(typeof(ShapeType));
            int randomIndex = Random.Range(0, values.Length);
            return values[randomIndex];
        }

    }
}
