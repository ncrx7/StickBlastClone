using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataModel
{
    [Serializable]
    public class ShapeWrapper<Ttype> where Ttype : Enum
    {
        public GameObject ShapePrefab;
        public Ttype Type;
        public int Weight;
    }
}
