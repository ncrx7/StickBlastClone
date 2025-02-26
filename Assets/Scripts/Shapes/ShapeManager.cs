using System.Collections;
using System.Collections.Generic;
using EntitiesData.Shapes;
using UnityEngine;

namespace Shapes
{
    public class ShapeManager : MonoBehaviour
    {
        [SerializeField] private ShapeData _shapeData;


        public ShapeData GetShapeData => _shapeData;
    }
}
