using System.Collections;
using System.Collections.Generic;
using DataModel;
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
        [SerializeField] private Transform _parentTransform;

        private Camera _mainCam;
        private NodeGridBoardManager _nodeGridBoardManager;
        private GameSettings _gameSettings;
        private ShapeManager _shapeManager;
        #endregion

        #region Fields
        [SerializeField] private float _pointerYOffset;
        private Vector3 _offset;
        private Vector3 _homePosition;
        [SerializeField] private Vector2Int _lastMousePositionOnGrid = new Vector2Int(-1, -1);
        #endregion

        [Inject]
        private void InitializeDependencies(NodeGridBoardManager nodeGridBoardManager, ShapeManager shapeManager, GameSettings gameSettings, Camera mainCam)
        {
            _nodeGridBoardManager = nodeGridBoardManager;
            _shapeManager = shapeManager;
            _gameSettings = gameSettings;
            _mainCam = mainCam;
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

            _parentTransform.localScale = _gameSettings.ShapeMovingScale;

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

            PathChecker.HandleCheckShapePathByPointer(_nodeGridBoardManager, _shapeManager, ref _lastMousePositionOnGrid);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_shapeManager.GetCanMoveFlag == false || !_shapeManager.IsDragging)
                return;

            _shapeManager.IsDragging = false;

            _parentTransform.localScale = _gameSettings.ShapeDefaultScale;

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
            var target = _mainCam.ScreenToWorldPoint(eventData.position);
            _offset = _parentTransform.position - target;
        }

        private void Move(PointerEventData eventData)
        {
            var target = _mainCam.ScreenToWorldPoint(eventData.position);
            target += _offset;
            target += Vector3.up * _pointerYOffset;
            target.z = 0;
            _parentTransform.position = target;
        }
        #endregion
    }
}
