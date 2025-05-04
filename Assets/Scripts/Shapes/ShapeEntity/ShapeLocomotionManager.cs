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
        [SerializeField] private ShapeManager _shapeManager;
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
        private void InitializeDependencies(NodeGridBoardManager nodeGridBoardManager)
        {
            _nodeGridBoardManager = nodeGridBoardManager;
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

            FindNearestNode();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_shapeManager.GetCanMoveFlag == false || !_shapeManager.IsDragging)
                return;

            _shapeManager.IsDragging = false;

            if (_shapeManager.GetCanPlaceFlag)
            {
                //_shapeManager.PlaceShape();
                MiniEventSystem.OnPlaceShape?.Invoke();
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

        private void FindNearestNode()
        {
            // Vector2 pointerWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);

            Vector2Int closestNodeCordinate = _nodeGridBoardManager.GetNodeGridSystem2D.GetXY(_shapeManager.GetHead.transform.position);


            if (closestNodeCordinate == _lastMousePositionOnGrid)
            {
                return;
            }

            Debug.Log("closest cordinate -> " + closestNodeCordinate.x + " - " + closestNodeCordinate.y);

            NodeManager closestNode = _nodeGridBoardManager.GetNodeGridSystem2D.GetValue(closestNodeCordinate.x, closestNodeCordinate.y)?.GetValue();

            if (closestNode == null)
                return;

            //_shapeManager.GetEdgesMatching.Clear();

            _lastMousePositionOnGrid = closestNodeCordinate;

            PathChecker.CheckPath(_shapeManager, closestNode, _shapeManager.GetEdgesMatching, false);
        }
        #endregion
    }
}
