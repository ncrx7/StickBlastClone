using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Vfx.Models
{
    [Serializable]
    public class VisualEffect
    {
        public VfxType VfxType;
        public GameObject VfxPrefab;
        public float Duration;
    }
}
