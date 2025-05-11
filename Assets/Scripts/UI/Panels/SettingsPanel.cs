using System.Collections;
using System.Collections.Generic;
using Data.Controllers;
using Data.Model;
using Enums;
using Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using Zenject;

namespace UI.MainMenu.Panels
{
    public class SettingsPanel : BasePanel<MainPanelType, GameData>
    {
        public TMP_Dropdown shapeHolderTypeDropDown;
        [Inject] private GameDataHandler _gameDataHandler;

        private void Start()
        {
            DropDownMatcher(shapeHolderTypeDropDown.value);
        }

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }

        public void OnDropDownValueChange()
        {
            int dropDownIndex = shapeHolderTypeDropDown.value;

            DropDownMatcher(dropDownIndex);
        }

        public void DropDownMatcher(int dropDownIndex)
        {
            switch (dropDownIndex)
            {
                case 0:
                    _gameDataHandler.shapeHolderType = ShapeHolderCreator.ShapeHolderType.UnOrdered;
                    break;
                case 1:
                    _gameDataHandler.shapeHolderType = ShapeHolderCreator.ShapeHolderType.Queue;
                    break;
                default:
                    Debug.LogError("Undefined dropdown value");
                    break;
            }
        }
    }
}
