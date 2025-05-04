using System.Collections;
using System.Collections.Generic;
using Data.Model;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;

namespace UI.MainMenu.Panels
{
    public class HomePanel : BasePanel<MainPanelType, GameData>
    {
        [SerializeField] private Button _startGameButton;

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }

        public Button GetStartGameButton => _startGameButton;
    }
}
