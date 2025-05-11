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
        public ShapeHolderCreator.ShapeHolderType ShapeHolderType;

        public GameData(string userName, int userLevel)
        {
            UserName = userName;
            UserLevel = userLevel;
        }
    }
}
