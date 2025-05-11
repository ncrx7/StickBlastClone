using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Model;
using Enums;
using Mainpanel;
using UI.MainMenu.Panels;
using UnityEngine;
using UnityUtils.BaseClasses;
using Zenject;

namespace Mainpanel
{
    public class UIManager : BaseUIManager<MainPanelType, GameData>
    {
        [SerializeField] private Vector3 _buttonScaleOnActive;
        [SerializeField] private GameObject _currentPanelDisplaying;
        [SerializeField] private GameObject _currentButtonObject;

        private LevelManager _levelManager;
        private GameDataHandler _gameDataHandler;

        [Inject]
        private void InitializeDependencies(LevelManager levelManager, GameDataHandler gameDataHandler)
        {
            _levelManager = levelManager;
            _gameDataHandler = gameDataHandler;
        }

        protected override void Awake()
        {
            base.Awake();

            InitializeUI();
        }

        private void OnEnable()
        {
            ExecuteUIAction(UIActionType.SetPanelVisibility, true, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);

            RegisterUIActions();
        }

        private void OnDisable()
        {
            UnRegisterUIActions();
        }

        private void RegisterUIActions()
        {
            MiniEventSystem.OnCompleteGameDataLoad += CompleteGameDataLoadUIBehaviour;
            MiniEventSystem.OnClickHomePanelButton += HomePanelButtonBehaviour;
            MiniEventSystem.OnClickLevelPanelButton += LevelPanelButtonBehaviour;
            MiniEventSystem.OnClickSettingsPanelButton += SettingsPanelButtonBehaviour;
            MiniEventSystem.OnClickStartGameButton += StartGameButtonBehaviour;
        }

        private void UnRegisterUIActions()
        {
            MiniEventSystem.OnCompleteGameDataLoad -= CompleteGameDataLoadUIBehaviour;
            MiniEventSystem.OnClickHomePanelButton -= HomePanelButtonBehaviour;
            MiniEventSystem.OnClickLevelPanelButton -= LevelPanelButtonBehaviour;
            MiniEventSystem.OnClickSettingsPanelButton -= SettingsPanelButtonBehaviour;
            MiniEventSystem.OnClickStartGameButton -= StartGameButtonBehaviour;
        }

        private void InitializeUI()
        {
            //CAN BE ADDED CUSTOM UI ACTIONS
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out var overlayPanel))
            {
                _currentButtonObject = overlayPanel.GetHomePanelButton.gameObject;
                //_currentButtonObject = GetPanel<OverlayPanel>(MainPanelType.OverlayPanel).GetHomePanelButton.gameObject;
            }

            _currentButtonObject.transform.localScale = _buttonScaleOnActive;

            _currentPanelDisplaying = _mainPanelMap[MainPanelType.HomePanel].gameObject;

            BindButtonActions();

            if (TryGetPanel<SettingsPanel>(MainPanelType.SettingsPanel, out var settingsPanel))
            {
                settingsPanel.DropDownMatcher(settingsPanel.shapeHolderTypeDropDown.value);
            }
        }

        private void CompleteGameDataLoadUIBehaviour(GameData gameData)
        {
            ExecuteUIAction(UIActionType.SetPanelVisibility, false, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);
        }

        private void BindButtonActions()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                overlayPanel.GetHomePanelButton.onClick.AddListener(() => MiniEventSystem.OnClickHomePanelButton?.Invoke());
                overlayPanel.GetLevelPanelButton.onClick.AddListener(() => MiniEventSystem.OnClickLevelPanelButton?.Invoke());
                overlayPanel.GetSettingsPanelButton.onClick.AddListener(() => MiniEventSystem.OnClickSettingsPanelButton?.Invoke());
            }

            if (TryGetPanel<HomePanel>(MainPanelType.HomePanel, out HomePanel homePanel))
            {
                homePanel.GetStartGameButton.onClick.AddListener(() => MiniEventSystem.OnClickStartGameButton?.Invoke());
            }
        }

        private void HomePanelButtonBehaviour()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                BasePanelButtonBehaviour(overlayPanel.GetHomePanelButton.gameObject, _mainPanelMap[MainPanelType.HomePanel]);
            }
        }

        private void LevelPanelButtonBehaviour()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                BasePanelButtonBehaviour(overlayPanel.GetLevelPanelButton.gameObject, _mainPanelMap[MainPanelType.LevelPanel]);
            }
        }

        private void SettingsPanelButtonBehaviour()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                BasePanelButtonBehaviour(overlayPanel.GetSettingsPanelButton.gameObject, _mainPanelMap[MainPanelType.SettingsPanel]);
            }
        }

        private void StartGameButtonBehaviour()
        {

            ExecuteUIAction(UIActionType.SetPanelVisibility, true, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);
            _levelManager.LoadSceneAsync(1).Forget();

        }

        private void BasePanelButtonBehaviour(GameObject buttonObject, BasePanel<MainPanelType, GameData> panelObject)
        {
            if (_currentButtonObject != null)
                _currentButtonObject.transform.localScale = Vector3.one;

            if (_currentPanelDisplaying != null)
            {
                ExecuteUIAction(UIActionType.SetPanelVisibility, false, _currentPanelDisplaying);
                panelObject.OnClosePanel(_gameDataHandler.GetGameDataObjectReference());
            }

            buttonObject.transform.localScale = _buttonScaleOnActive;
            _currentButtonObject = buttonObject.gameObject;

            _currentPanelDisplaying = panelObject.gameObject;

            ExecuteUIAction(UIActionType.SetPanelVisibility, true, _currentPanelDisplaying);

            panelObject.OnOpenPanel(_gameDataHandler.GetGameDataObjectReference());
        }

        public void StartTheLevelButton()
        {
            _levelManager.LoadSceneAsync(1).Forget();
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}
