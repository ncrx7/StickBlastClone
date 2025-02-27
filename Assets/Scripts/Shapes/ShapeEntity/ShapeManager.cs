using System.Collections;
using System.Collections.Generic;
using EntitiesData.Shapes;
using NodeGridSystem.Controllers;
using UnityEngine;

namespace Shapes
{
    public class ShapeManager : MonoBehaviour
    {
        [SerializeField] private ShapeData _shapeData;
        private List<EdgeManager> _edgesMatching = new();
        [SerializeField] private bool _canPlace;
        [SerializeField] private bool _canMove = false;
        public bool IsDragging;

        private void OnEnable()
        {
            MiniEventSystem.OnPlaceShape += PlaceShape;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnPlaceShape -= PlaceShape;
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
            if(GetEdgesMatching.Count == 0)
                return;

            EdgeManager firstEdge = GetEdgesMatching[0];

            foreach (var edge in GetEdgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 1f;
                targetDisplayColor = new Color(255, 0, 197);
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;

                edge.IsEmpty = false;
            }

            NodeGridBoardManager.Instance.CheckMidCellFullnessOnBoard();
        }

        public ShapeData GetShapeData => _shapeData;
        public List<EdgeManager> GetEdgesMatching => _edgesMatching;
        public bool GetCanPlaceFlag => _canPlace;
        public bool GetCanMoveFlag => _canMove;
    }
}
