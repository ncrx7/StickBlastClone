using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityUtils.Core.VfxSystem;

namespace DataModel
{
    [Serializable]
    public class GameSettings 
    {
        public List<ShapeWrapper<ShapeType>> ShapeData;
        public List<VfxWrapper<VfxType>> VfxData;
    }
}
