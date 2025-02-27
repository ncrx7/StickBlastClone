using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Shapes
{
    public class ShapeLocomotionManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        #region References
        [SerializeField] private ShapeManager _shapeManager;
        [SerializeField] private Transform _parentTransform;
        #endregion

        #region Variable
        private Vector3 _offset;
        private Vector3 _homePosition;
        #endregion

        #region MonoBeheviour Callbacks
        private void Start()
        {
            _homePosition = _parentTransform.position;
        }
        #endregion

        #region Interface Methods
        public void OnPointerDown(PointerEventData eventData)
        {
            _shapeManager.IsDragging = true;
            SetOffset(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            Move(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _shapeManager.IsDragging = false;

            if(_shapeManager.GetCanPlaceFlag)
            {
                //_shapeManager.PlaceShape();
                MiniEventSystem.OnPlaceShape?.Invoke();
                transform.parent.gameObject.SetActive(false);
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
            target.z = 0;
            _parentTransform.position = target;
        }
        #endregion
    }
}
