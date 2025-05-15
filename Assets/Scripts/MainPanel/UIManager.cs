using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Controllers;
using Data.Model;
using DG.Tweening;
using Enums;
using Mainpanel;
using StateMachine;
using UI.MainMenu.Panels;
using UnityEngine;
using UnityUtils.BaseClasses;
using Zenject;

namespace Mainpanel
{
    public class UIManager : BaseUIManager<MainPanelType, GameData>
    {
        private StateMachineController _stateMachine;

        [SerializeField] private Vector3 _buttonScaleOnInactive;
        [SerializeField] private Vector3 _buttonScaleOnActive;
        [SerializeField] private float _buttonScaleAnimationDuration;
        [SerializeField] private BasePanel<MainPanelType, GameData> _currentPanelDisplaying;
        [SerializeField] private GameObject _currentButtonObject;

        [SerializeField] private RectTransform _leftTransform, _rightTransform, _midTransform, _upTransform;

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

            Application.targetFrameRate = 120;

            InitializeUI();

            HandleStateMachine();
        }

        private void OnEnable()
        {
            ExecuteUIAction(UIActionType.SetPanelVisibility, true, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);

            if (_gameDataHandler.IsDataLoadFinished) // ana sahne tekrar yuklendiginde eger datalar zaten yuklu ise loading paneli kapatıyorum.
                ExecuteUIAction(UIActionType.SetPanelVisibility, false, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);

            RegisterUIActions();
        }

        private void OnDisable()
        {
            UnRegisterUIActions();
        }

        private void HandleStateMachine()
        {
            _stateMachine = new StateMachineController();

            var homeState = new HomePanelDisplayingState();
            var shopState = new ShopPanelDisplayingState();
            var settingsState = new SettingsPanelDisplayingState();

            _stateMachine.AddState(homeState);
            _stateMachine.AddState(shopState);
            _stateMachine.AddState(settingsState);

            _stateMachine.SetInitialState<HomePanelDisplayingState>();
        }

        private void RegisterUIActions()
        {
            MiniEventSystem.OnCompleteGameDataLoad += CompleteGameDataLoadUIBehaviour;
            MiniEventSystem.OnClickHomePanelButton += HomePanelButtonBehaviour;
            MiniEventSystem.OnClickLevelPanelButton += ShopPanelButtonBehaviour;
            MiniEventSystem.OnClickSettingsPanelButton += SettingsPanelButtonBehaviour;
            MiniEventSystem.OnClickStartGameButton += StartGameButtonBehaviour;
        }

        private void UnRegisterUIActions()
        {
            MiniEventSystem.OnCompleteGameDataLoad -= CompleteGameDataLoadUIBehaviour;
            MiniEventSystem.OnClickHomePanelButton -= HomePanelButtonBehaviour;
            MiniEventSystem.OnClickLevelPanelButton -= ShopPanelButtonBehaviour;
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

            _currentButtonObject.transform.DOScale(_buttonScaleOnActive, _buttonScaleAnimationDuration);

            _currentPanelDisplaying = _mainPanelMap[MainPanelType.HomePanel];

            BindButtonActions();
        }

        private void CompleteGameDataLoadUIBehaviour(GameData gameData)
        {
            ExecuteUIAction(UIActionType.SetPanelVisibility, false, _mainPanelMap[MainPanelType.LoadingPanel].gameObject);

            if (TryGetPanel<HomePanel>(MainPanelType.HomePanel, out var homePanel))
            {
                homePanel.OnOpenPanel(gameData);
            }
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
                
                _stateMachine.ChangeState<HomePanelDisplayingState>();
            }
        }

        private void ShopPanelButtonBehaviour()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                BasePanelButtonBehaviour(overlayPanel.GetLevelPanelButton.gameObject, _mainPanelMap[MainPanelType.LevelPanel]);

                _stateMachine.ChangeState<ShopPanelDisplayingState>();
            }
        }

        private void SettingsPanelButtonBehaviour()
        {
            if (TryGetPanel<OverlayPanel>(MainPanelType.OverlayPanel, out OverlayPanel overlayPanel))
            {
                BasePanelButtonBehaviour(overlayPanel.GetSettingsPanelButton.gameObject, _mainPanelMap[MainPanelType.SettingsPanel]);

                _stateMachine.ChangeState<SettingsPanelDisplayingState>();
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
                _currentButtonObject.transform.DOScale(_buttonScaleOnInactive, _buttonScaleAnimationDuration);

            if (_currentPanelDisplaying != null)
            {
                //ExecuteUIAction(UIActionType.SetPanelVisibility, false, _currentPanelDisplaying);
                AnimatePanel(_currentPanelDisplaying, panelObject, true);
                panelObject.OnClosePanel(_gameDataHandler.GetGameDataObjectReference());
            }

            buttonObject.transform.DOScale(_buttonScaleOnActive, _buttonScaleAnimationDuration);
            _currentButtonObject = buttonObject.gameObject;

            _currentPanelDisplaying = panelObject;

            //ExecuteUIAction(UIActionType.SetPanelVisibility, true, _currentPanelDisplaying);
            AnimatePanel(_currentPanelDisplaying, panelObject, false);

            panelObject.OnOpenPanel(_gameDataHandler.GetGameDataObjectReference());
        }

        private void AnimatePanel(BasePanel<MainPanelType, GameData> panelToClose, BasePanel<MainPanelType, GameData> panelToOpen, bool willClose)
        {
            if (willClose)
            {
                switch (panelToClose.positionType)
                {
                    case PanelPositionType.Left:
                        panelToClose.GetRectTransform.DOAnchorPos(_leftTransform.anchoredPosition, 0.5f).SetEase(Ease.InOutCubic);
                        break;
                    case PanelPositionType.Right:
                        panelToClose.GetRectTransform.DOAnchorPos(_rightTransform.anchoredPosition, 0.5f).SetEase(Ease.InOutCubic);
                        break;
                    case PanelPositionType.Mid:
                        panelToClose.GetRectTransform.DOAnchorPos(_upTransform.anchoredPosition, 0.5f).SetEase(Ease.InOutCubic);
                        break;
                    default:
                        Debug.LogWarning("Undefined panel position type!!");
                        break;
                }
            }
            else
            {
                panelToClose.GetRectTransform.DOAnchorPos(_midTransform.anchoredPosition, 0.5f).SetEase(Ease.InOutCubic);
            }
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
