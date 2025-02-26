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
        public bool IsDragging;

        public void SetCanPlaceFlag(bool flag)
        {
            _canPlace = flag;
        }

        public void PlaceShape()
        {
            foreach (var edge in GetEdgesMatching)
            {
                Color targetDisplayColor = edge.GetBlockShapeSpriteRenderer.color;
                targetDisplayColor.a = 1f;
                targetDisplayColor = new Color(255, 0, 197);
                edge.GetBlockShapeSpriteRenderer.color = targetDisplayColor;

                edge.IsEmpty = false;
            }
        }

        public ShapeData GetShapeData => _shapeData;
        public List<EdgeManager> GetEdgesMatching => _edgesMatching;
        public bool GetCanPlaceFlag => _canPlace;
    }
}
