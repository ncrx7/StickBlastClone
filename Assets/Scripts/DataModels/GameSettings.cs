using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace DataModel
{
    [Serializable]
    public class GameSettings 
    {
        public List<ShapeWrapper<ShapeType>> ShapeData;
    }
}
