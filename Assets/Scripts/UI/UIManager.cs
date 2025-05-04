using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EntitiesData.Levels;
using Enums;
using Mainpanel;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("References")]
        private GameManager _gameManager;
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _gameOverPanelNoMatching;
        [SerializeField] private GameObject _gameOverPanelTimeOut;
        [SerializeField] private GameObject _gameSuccessPanel;
        [SerializeField] private TextMeshProUGUI _levelReachScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private Slider _slider;

        [Header("Settings")]
        [SerializeField] private float _comboTextAnimationTime;
        [SerializeField] private Vector3 _comboTextScaleFactor;

        private LevelManager _levelManager;

        [Inject]
        private void InitializeDependencies(LevelManager levelManager, GameManager gameManager)
        {
            _levelManager = levelManager;
            _gameManager = gameManager;
        }


        private void OnEnable()
        {
            MiniEventSystem.OnStartGame += SetInitialTextsOnScene;
            MiniEventSystem.OnEndGame += ActivateGameEndPanel;
            MiniEventSystem.IncreaseScore += UpdateScoreUI;
            MiniEventSystem.ActivateLoadingUI += HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI += DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork += UpdateTimer;
            MiniEventSystem.OnStartGame += UpdateLevelText;
            MiniEventSystem.OnComboIncrease += HandleComboText;

            UpdateScoreUI(0);
        }

        private void OnDisable()
        {
            MiniEventSystem.OnEndGame -= ActivateGameEndPanel;
            MiniEventSystem.OnStartGame -= SetInitialTextsOnScene;
            MiniEventSystem.IncreaseScore -= UpdateScoreUI;
            MiniEventSystem.ActivateLoadingUI -= HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI -= DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork -= UpdateTimer;
            MiniEventSystem.OnStartGame -= UpdateLevelText;
            MiniEventSystem.OnComboIncrease -= HandleComboText;
        }

        private void UpdateTimer(int time)
        {
            _timer.text = time.ToString();
        }

        private void UpdateLevelText()
        {
            _levelText.text = "Level " + (_levelManager.GetLevel + 1).ToString();
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
                    _gameSuccessPanel.SetActive(true);
                    MiniEventSystem.PlaySoundClip?.Invoke(SoundType.SucessEnd);
                    break;
                default:
                    Debug.LogWarning("Undefined gameEndId!!");
                    break;
            }


            _gameManager.IsGamePaused = true;
        }

        private void SetInitialTextsOnScene()
        {
            _levelReachScore.text = _gameManager.GetLevelData.LevelReachScore.ToString();
        }

        private void UpdateScoreUI(int newScore)
        {
            _score.text = newScore.ToString();
            _slider.value = (float)newScore / _gameManager.GetLevelData.LevelReachScore;
        }

        private void HandleComboText()
        {
            _comboText.gameObject.SetActive(true);
            _comboText.transform.localScale = Vector3.zero;
            _comboText.text = "Combo " + ComboManager.Instance.GetCurrentComboAmount.ToString();

            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.Combo);

            _comboText.transform.DOScale(_comboTextScaleFactor, _comboTextAnimationTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    _comboText.transform.DOScale(Vector3.zero, _comboTextAnimationTime)
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

        public void HandleActivaiateLoadingUI()
        {
            _loadingPanel.SetActive(true);
        }

        public void DeActivaiteLoadingUI()
        {
            _loadingPanel.SetActive(false);
        }
    }
}
