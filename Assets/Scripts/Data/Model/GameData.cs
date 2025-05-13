using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Data.Model
{
    public class GameData 
    {
        public string UserName;
        public int UserLevel;
        public bool FirstEntry;
        public Settings settings;
    
        public GameData(string userName, int userLevel)
        {
            UserName = userName;
            UserLevel = userLevel;
        }
    }

    [Serializable]
    public class Settings
    {
        public ShapeHolderCreator.ShapeHolderType ShapeHolderType;
        public int GridWidth;
        public int GridHeight;
    }
}
