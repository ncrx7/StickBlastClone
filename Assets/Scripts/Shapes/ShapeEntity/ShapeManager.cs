using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitiesData.Shapes;
using Enums;
using NodeGridSystem.Controllers;
using Unity.VisualScripting;
using UnityEngine;

namespace Shapes
{
    public class ShapeManager : MonoBehaviour
    {
        [SerializeField] private List<SpriteRenderer> _spriteRendererList;
        [SerializeField] private ShapeData _shapeData;
        private List<EdgeManager> _edgesMatching = new();
        [SerializeField] private bool _canPlace;
        [SerializeField] private bool _canMove = false;
        public bool IsDragging;

        public Action _placeCallBack;

        private void OnEnable()
        {
            MiniEventSystem.OnPlaceShape += PlaceShape;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= PlaceShape;
        }

        private void Start()
        {
            foreach (var spriteRenderer in _spriteRendererList)
            {
                spriteRenderer.color = GameManager.Instance.GetLevelData.LevelColor;
            }
        }

        public void Setup(Action placeCallBack, Vector3 targetPosition)
        {
            transform.position = targetPosition;
            _placeCallBack = placeCallBack;
        }

        public void SetCanPlaceFlag(bool flag)
        {
            _canPlace = flag;
        }

        public void SetCanMoveFlag(bool flag)
        {
            _canMove = flag;
        }

        public void PlaceShape()
        {
            if (GetEdgesMatching.Count == 0)
                return;

            EdgeManager firstEdge = GetEdgesMatching[0];

            foreach (var edge in GetEdgesMatching)
            {
                edge.PaintEdge();
                edge.IsEmpty = false;
                edge.StartNode.PaintNode();
                edge.EndNode.PaintNode();

                MiniEventSystem.PlayVfx?.Invoke(edge.transform.position, VfxType.Place);
            }

            NodeGridBoardManager.Instance.CheckMidCellFullnessOnBoard();
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.PlaceShape);

            _placeCallBack?.Invoke();
            //Destroy(this.gameObject);
        }

        public bool CheckRelativeMatchExist()
        {
            for (int x = 0; x < NodeGridBoardManager.Instance.GetWidth; x++)
            {
                for (int y = 0; y < NodeGridBoardManager.Instance.GetHeight; y++)
                {
                    var gridNodeObject = NodeGridBoardManager.Instance.GetNodeGridSystem2D.GetValue(x, y);

                    NodeManager nodeManager = gridNodeObject.GetValue();

                    if (nodeManager.GetNodeCollisionManager.CheckShapePath(this, GetEdgesMatching, true))
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public void Reset()
        {
            SetCanMoveFlag(false);
            SetCanPlaceFlag(false);
            _placeCallBack = null;
            _edgesMatching.Clear();
        }

        public ShapeData GetShapeData => _shapeData;
        public List<EdgeManager> GetEdgesMatching => _edgesMatching;
        public bool GetCanPlaceFlag => _canPlace;
        public bool GetCanMoveFlag => _canMove;
    }
}
