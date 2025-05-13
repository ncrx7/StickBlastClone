using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Sounds.Models;
using UnityEngine;
using UnityUtils.Core.VfxSystem;

namespace DataModel
{
    [Serializable]
    public class GameSettings
    {
        [Header("Pool Data")]
        public List<ShapeWrapper<ShapeType>> ShapeData;
        public List<VfxWrapper<VfxType>> VfxData;

        [Header("Screen usage rate")]
        [Range(0.1f, 1f)]
        public float XScreenUsageRate, YScreenUsageRate;

        [Header("Grid Board Data")]
/*         public int Width = 6;
        public int height = 6; */
        public Vector3 OffsetFromCenter = Vector3.zero;
        public bool Debug = true;

        [Header("Shape Queue Data")]
        public float Margin;
        public float AnimationTime;

        [Header("Shape Scale Data")]
        public Vector2 ShapeDefaultScale;
        public Vector2 ShapeMovingScale;

        [Header("Combo Settings")]
        public float ComboTextAnimationTime;
        public Vector3 ComboTextScaleFactor;

        [Header("Sounds Data")]
        public List<Sound> Sounds;

    }
}
