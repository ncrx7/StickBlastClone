using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace DataModel
{
    [Serializable]
    public class ShapeWrapper<Ttype> where Ttype : Enum
    {
        public ShapeManager ShapePrefab;
        public Ttype Type;
        public int Weight;
    }
}
