using System.Collections;
using System.Collections.Generic;
using Enums;
using NodeGridSystem.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Shapes
{
    public class ShapeLocomotionManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region References
        private ShapeManager _shapeManager;
        [SerializeField] private Transform _parentTransform;
        private NodeGridBoardManager _nodeGridBoardManager;
        #endregion

        #region Fields
        [SerializeField] private float _pointerYOffset;
        private Vector3 _offset;
        private Vector3 _homePosition;
        private Vector2Int _lastMousePositionOnGrid = new Vector2Int(-1, -1);
        #endregion

        [Inject]
        private void InitializeDependencies(NodeGridBoardManager nodeGridBoardManager, ShapeManager shapeManager)
        {
            _nodeGridBoardManager = nodeGridBoardManager;
            _shapeManager = shapeManager;
        }

        #region MonoBeheviour Callbacks
        private void Start()
        {
            _homePosition = _parentTransform.position;
        }
        #endregion

        #region Interface Methods
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_shapeManager.GetCanMoveFlag == false)
                return;

            _shapeManager.IsDragging = true;
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.SelectShape);

            SetOffset(eventData);
            _homePosition = _parentTransform.position;

            Move(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (_shapeManager.GetCanMoveFlag == false || !_shapeManager.IsDragging)
            {
                //Debug.LogWarning("CAN T ON DRAG BECAUSE MOVE FLAG FALSE");
                return;
            }

            Move(eventData);

            PathChecker.HandleCheckShapePathByPointer(_nodeGridBoardManager, _shapeManager, _lastMousePositionOnGrid);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_shapeManager.GetCanMoveFlag == false || !_shapeManager.IsDragging)
                return;

            _shapeManager.IsDragging = false;

            if (_shapeManager.GetCanPlaceFlag)
            {
                _shapeManager.PlaceShape();
                
                MiniEventSystem.OnPlaceShape?.Invoke(_shapeManager);
                //transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _parentTransform.position = _homePosition;
            }
        }
        #endregion

        #region Private Methods
        private void SetOffset(PointerEventData eventData)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            _offset = _parentTransform.position - target;
        }

        private void Move(PointerEventData eventData)
        {
            var target = Camera.main.ScreenToWorldPoint(eventData.position);
            target += _offset;
            target += Vector3.up * _pointerYOffset;
            target.z = 0;
            _parentTransform.position = target;
        }
        #endregion
    }
}
