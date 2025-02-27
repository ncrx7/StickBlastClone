using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Shapes
{
    public class ShapeQueueManager : MonoBehaviour
    {
        [SerializeField] private Queue<ShapeManager> _shapeQueue = new();
        [SerializeField] private List<ShapeManager> _shapePrefaps;
        [SerializeField] private Transform _queueStartingPoint;
        [SerializeField] private Transform _queueEndPoint;
        [SerializeField] private float _margin;
        [SerializeField] private float _animationTime;

        private Vector3 _currentPosition;

        private void OnEnable()
        {
            MiniEventSystem.OnPlaceShape += () => DequeueShape().Forget();
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= () => DequeueShape().Forget();
        }

        private async void Start()
        {
            _currentPosition = _queueStartingPoint.position;

            await CreateQueue();
            await RelocationShapes();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RelocationShapes().Forget();
            }
        }

        private async UniTask CreateQueue()
        {
            for (int i = 0; i < 6; i++)
            {
                ShapeManager shape = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, transform);
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
                shape.transform.DOMove(_currentPosition, _animationTime).OnComplete(() =>
                {
                    if (index == 0)
                        shape.SetCanMoveFlag(true);

                    index++;
                }
                );
                //shape.transform.position = _currentPosition;
                _currentPosition.x -= _margin;

                await UniTask.Delay(50);
            }
        }

        private async UniTask DequeueShape()
        {
            _shapeQueue.Dequeue();

            ShapeManager shapeSpawned = Instantiate(GetRandomShape(), _queueEndPoint.transform.position, Quaternion.identity, transform);
            _shapeQueue.Enqueue(shapeSpawned);

            await RelocationShapes();

        }

        private ShapeManager GetRandomShape()
        {
            return _shapePrefaps[Random.Range(0, _shapePrefaps.Count)];
        }
    }
}
