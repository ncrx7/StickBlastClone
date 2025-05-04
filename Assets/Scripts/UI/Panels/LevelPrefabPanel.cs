using Data.Model;
using Enums;
using Mainpanel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using Zenject;

namespace UI.MainMenu.Panels
{
    public class LevelPrefabPanel : BasePanel<MainPanelType, GameData>
    {
        [SerializeField] private TextMeshProUGUI _label;
        public string Label { get; set; }

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

            _label.text = Label;
        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }
    }
}
