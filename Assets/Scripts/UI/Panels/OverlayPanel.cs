using System.Collections;
using System.Collections.Generic;
using Data.Model;
using Enums;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;

namespace UI.MainMenu.Panels
{
    public class OverlayPanel : BasePanel<MainPanelType, GameData>
    {
        [SerializeField] private Button _homePanelButton;
        [SerializeField] private Button _levelPanelButton;
        [SerializeField] private Button _settingsPanelButton;

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }

        public Button GetHomePanelButton => _homePanelButton;
        public Button GetLevelPanelButton => _levelPanelButton;
        public Button GetSettingsPanelButton => _settingsPanelButton;
    }
}
