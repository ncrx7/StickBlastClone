using System.Collections;
using System.Collections.Generic;
using Data.Model;
using Enums;
using Mainpanel;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using Zenject;

namespace UI.MainMenu.Panels
{
    public class LevelPanel : BasePanel<MainPanelType, GameData>
    {
        [SerializeField] private Transform _levelContainer;
        [SerializeField] private LevelPrefabPanel _levelPrefab;
        private bool _levelPrefabsLoaded = false;

        [Inject] private LevelManager _levelManager;

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);
/* 
            if(_levelPrefabsLoaded)
                return;

            foreach (var levelData in _levelManager.GetAllLevelData)
            {
                LevelPrefabPanel levelPrefab = Instantiate(_levelPrefab, _levelContainer);
                levelPrefab.Label = "Level " + levelData.Level;
                levelPrefab.OnOpenPanel(gameData);
            }
            
            _levelPrefabsLoaded = true; //TODO: MAKE HERE FROM PANEL MEMORY POOL AND MAKE IT DYNAMIC WHEN CLOSE OPEN */
        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }
    }
}
