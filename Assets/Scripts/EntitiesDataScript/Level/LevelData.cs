using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitiesData.Levels
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public Color LevelColor;
        public int LevelReachScore;
        public int Timer;
        public int Level;
    }
}
