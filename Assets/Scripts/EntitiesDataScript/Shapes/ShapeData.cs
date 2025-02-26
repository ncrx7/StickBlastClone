using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace EntitiesData.Shapes
{
    [CreateAssetMenu(fileName = "ShapeData", menuName = "ScriptableObjects/ShapeData", order = 1)]
    public class ShapeData : ScriptableObject
    {
        public ShapeType ShapeType;
        public List<Direction> ShapeDirections;
    }
}
