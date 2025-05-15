using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Model;
using DataModel;
using DG.Tweening;
using EntitiesData.Levels;
using Enums;
using Level;
using Mainpanel;
using Shapes;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityUtils.StaticHelpers;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        private GameManager _gameManager;
        private ComboManager _comboManager;

        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _gameOverPanelNoMatching;
        [SerializeField] private GameObject _gameOverPanelTimeOut;
        [SerializeField] private GameObject _gameSuccessPanel;
        [SerializeField] private GameObject _queueLastItemPanel;

        [SerializeField] private TextMeshProUGUI _levelReachScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _comboText;

        [SerializeField] private Button _returnMainMenuButton, _noMatchHomeButton, _timeOutloseHomeButton, _successHomeButton;


        [SerializeField] private Image _sliderLine, _gameSuccessSliderLine;

        private GameSettings _gameSettings;
        private LevelManager _levelManager;

        [Header("Tweens")]
        private Tween _scoreSliderTween, _gameSucessPanelSliderTween;

        [SerializeField] int _numberAnimatorDelay;

        [Inject]
        private void InitializeDependencies(LevelManager levelManager, GameManager gameManager, ComboManager comboManager, GameSettings gameSettings)
        {
            _levelManager = levelManager;
            _gameManager = gameManager;
            _comboManager = comboManager;
            _gameSettings = gameSettings;
        }


        private void OnEnable()
        {
            _returnMainMenuButton.onClick.AddListener(LoadMainScene);
            _noMatchHomeButton.onClick.AddListener(LoadMainScene);
            _timeOutloseHomeButton.onClick.AddListener(LoadMainScene);
            _successHomeButton.onClick.AddListener(LoadMainScene);

            MiniEventSystem.OnStartGame += InitializeItems;
            MiniEventSystem.OnEndGame += ActivateGameEndPanel;
            MiniEventSystem.IncreaseScore += UpdateScoreItems;
            MiniEventSystem.ActivateLoadingUI += HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI += DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork += UpdateTimer;
            MiniEventSystem.OnStartGame += UpdateLevelText;
            MiniEventSystem.OnComboIncrease += HandleComboText;
            MiniEventSystem.OnShapeHolderServiceSetted += SetShapeHolderServiceUI;

            UpdateScoreItems(0, 0);
        }

        private void OnDisable()
        {
            MiniEventSystem.OnEndGame -= ActivateGameEndPanel;
            MiniEventSystem.OnStartGame -= InitializeItems;
            MiniEventSystem.IncreaseScore -= UpdateScoreItems;
            MiniEventSystem.ActivateLoadingUI -= HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI -= DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork -= UpdateTimer;
            MiniEventSystem.OnStartGame -= UpdateLevelText;
            MiniEventSystem.OnComboIncrease -= HandleComboText;
            MiniEventSystem.OnShapeHolderServiceSetted -= SetShapeHolderServiceUI;

            _returnMainMenuButton.onClick.RemoveAllListeners();
            _noMatchHomeButton.onClick.RemoveAllListeners();
            _timeOutloseHomeButton.onClick.RemoveAllListeners();
            _successHomeButton.onClick.RemoveAllListeners();
        }

        private void UpdateTimer(int time)
        {
            _timer.text = time.ToString();
        }

        private void UpdateLevelText()
        {
            _levelText.text = "LVL." + _levelManager.GetLevel.ToString();
        }

        private void ActivateGameEndPanel(int gameEndID)
        {
            if (_gameManager.IsGamePaused)
                return;

            switch (gameEndID)
            {
                case 0:
                    _gameOverPanelNoMatching.SetActive(true);

                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.End);

                    break;
                case 1:
                    _gameOverPanelTimeOut.SetActive(true);

                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.End);

                    break;
                case 2:
                    _gameSuccessSliderLine.fillAmount = 0;

                    AnimateSlider(_gameSuccessSliderLine, ref _gameSucessPanelSliderTween, _levelManager.GetLevel, 5f);

                    _gameSuccessPanel.SetActive(true);

                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.SucessEnd);

                    break;
                default:
                    Debug.LogWarning("Undefined gameEndId!!");
                    
                    break;
            }


            _gameManager.IsGamePaused = true;
        }

        private void InitializeItems()
        {
            _levelReachScore.text = _gameManager.GetLevelData.LevelReachScore.ToString();
            _sliderLine.color = _gameManager.GetLevelData.LevelColor;
            _score.text = _gameManager.GetScore.ToString();
            _sliderLine.fillAmount = 0;

        }

        private void UpdateScoreItems(int oldScore, int newScore)
        {
            AnimateNumberText(oldScore, newScore, _score);

            AnimateSlider(_sliderLine, ref _scoreSliderTween, newScore, _gameManager.GetLevelData.LevelReachScore);
        }

        private void HandleComboText()
        {
            _comboText.gameObject.SetActive(true);
            _comboText.transform.localScale = Vector3.zero;
            _comboText.text = "Combo " + _comboManager.GetCurrentComboAmount.ToString();

            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.Combo);

            _comboText.transform.DOScale(_gameSettings.ComboTextScaleFactor, _gameSettings.ComboTextAnimationTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _comboText.transform.DOScale(Vector3.zero, _gameSettings.ComboTextAnimationTime)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => _comboText.gameObject.SetActive(false));
                });
        }

        public void NextLevelButton()
        {
            _levelManager.NextLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public async void LoadMainScene()
        {
            _loadingPanel.SetActive(true);
            await SceneLoader.LoadSceneAsync(0);
        }

        public void HandleActivaiateLoadingUI()
        {
            _loadingPanel.SetActive(true);
        }

        public void DeActivaiteLoadingUI()
        {
            _loadingPanel.SetActive(false);
        }

        private void AnimateSlider(Image sliderLine, ref Tween tween, int _currentValue, float totalValue)
        {
            if (tween != null)
                tween.Kill();

            float sliderValue = _currentValue / totalValue;

            tween = sliderLine.DOFillAmount(sliderValue, 1).SetEase(Ease.OutCubic);
        }

        private async void AnimateNumberText(int startNum, int endNum, TextMeshProUGUI textMesh)
        {
            int currentNum = startNum;

            while (currentNum < endNum)
            {
                textMesh.text = currentNum.ToString();
                currentNum++;

                await UniTask.Delay(_numberAnimatorDelay);
            }
        }

        private void SetShapeHolderServiceUI(ShapeHolderCreator.ShapeHolderType type)
        {
            switch (type)
            {
                case ShapeHolderCreator.ShapeHolderType.UnOrdered:
                    _queueLastItemPanel.SetActive(false);
                    break;
                case ShapeHolderCreator.ShapeHolderType.Queue:
                    _queueLastItemPanel.SetActive(true);
                    break;
                default:
                    Debug.LogWarning("Undefined holder service type!!");
                    break;
            }
        }
    }
}
