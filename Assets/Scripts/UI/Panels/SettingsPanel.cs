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
        public TMP_Dropdown shapeHolderTypeDropDown, gridSizeDropDown;
        [Inject] private GameDataHandler _gameDataHandler;

        private void OnEnable()
        {
            MiniEventSystem.OnCompleteGameDataLoad += InitializeDropDowns;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnCompleteGameDataLoad -= InitializeDropDowns;
        }

        public override void OnOpenPanel(GameData gameData)
        {
            base.OnOpenPanel(gameData);

        }

        public override void OnClosePanel(GameData gameData)
        {
            base.OnClosePanel(gameData);

        }

        public void OnShapeHolderDropDownValueChange()
        {
            int dropDownIndex = shapeHolderTypeDropDown.value;

            ShapeHolderDropDownMatcher(dropDownIndex);
        }

        public void OnGridSizeDropDownValueChange()
        {
            int dropDownIndex = gridSizeDropDown.value;

            GridSizeDropDownMatcher(dropDownIndex);
        }

        public void InitializeDropDowns(GameData gameData)
        {
            ShapeHolderDropDownMatcher((int)gameData.settings.ShapeHolderType);
            
            GridSizeDropDownMatcher(GridSizeToDropdownValue(gameData.settings.GridWidth));
        }

        public void ShapeHolderDropDownMatcher(int dropDownIndex)
        {
            shapeHolderTypeDropDown.value = dropDownIndex;

            switch (dropDownIndex)
            {
                case 0:
                    _gameDataHandler.GetGameDataObjectReference().settings.ShapeHolderType = ShapeHolderCreator.ShapeHolderType.UnOrdered;
                    break;
                case 1:
                    _gameDataHandler.GetGameDataObjectReference().settings.ShapeHolderType = ShapeHolderCreator.ShapeHolderType.Queue;
                    break;
                default:
                    Debug.LogError("Undefined dropdown value");
                    break;
            }

            _gameDataHandler.UpdateGameDataFile();
        }

        public void GridSizeDropDownMatcher(int dropDownIndex)
        {
            gridSizeDropDown.value = dropDownIndex;

            switch (dropDownIndex)
            {
                case 0:
                    _gameDataHandler.GetGameDataObjectReference().settings.GridWidth = 4;
                    _gameDataHandler.GetGameDataObjectReference().settings.GridHeight = 4;
                    break;
                case 1:
                    _gameDataHandler.GetGameDataObjectReference().settings.GridWidth = 6;
                    _gameDataHandler.GetGameDataObjectReference().settings.GridHeight = 6;
                    break;
                case 2:
                    _gameDataHandler.GetGameDataObjectReference().settings.GridWidth = 12;
                    _gameDataHandler.GetGameDataObjectReference().settings.GridHeight = 12;
                    break;
                case 3:
                    _gameDataHandler.GetGameDataObjectReference().settings.GridWidth = 24;
                    _gameDataHandler.GetGameDataObjectReference().settings.GridHeight = 24;
                    break;
                case 4:
                    _gameDataHandler.GetGameDataObjectReference().settings.GridWidth = 36;
                    _gameDataHandler.GetGameDataObjectReference().settings.GridHeight = 36;
                    break;
                default:
                    Debug.LogError("Undefined dropdown value");
                    break;
            }

            _gameDataHandler.UpdateGameDataFile();
        }

        private int GridSizeToDropdownValue(int width)
        {
            switch (width)
            {
                case 4:
                    return 0;
                case 6:
                    return 1;
                case 12:
                    return 2;
                case 24:
                    return 3;
                case 36:
                    return 4;
                default:
                    Debug.LogWarning("Undefined width value");
                    return -1;
            }
        }
    }
}
