using System.Collections;
using System.Collections.Generic;
using EntitiesData.Shapes;
using UnityEngine;

namespace Shapes
{
    public class ShapeManager : MonoBehaviour
    {
        [SerializeField] private ShapeData _shapeData;
        [SerializeField] private bool _canPlace;

        public void SetCanPlaceFlag(bool flag)
        {
            _canPlace = flag;
        }

        public ShapeData GetShapeData => _shapeData;
    }
}
