using System.Collections;
using System.Collections.Generic;
using EntitiesData.Levels;
using UnityEngine;

namespace InterfaceServices.Level
{
    public interface IGameManager
    {
        public void SetLevelData(LevelData levelData);
        public void HandleIncreaseScore(int newScore);
    }
}
