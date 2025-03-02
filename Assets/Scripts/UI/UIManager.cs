using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Mainpanel;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _gameOverPanelNoMatching;
        [SerializeField] private GameObject _gameOverPanelTimeOut;
        [SerializeField] private GameObject _gameSuccessPanel;
        [SerializeField] private TextMeshProUGUI _levelReachScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Slider _slider;



        private void OnEnable()
        {
            MiniEventSystem.OnStartGame += SetInitialTextsOnScene;
            MiniEventSystem.OnEndGame += ActivateGameEndPanel;
            MiniEventSystem.IncreaseScore += UpdateScoreUI;
            MiniEventSystem.ActivateLoadingUI += HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI += DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork += UpdateTimer;
            MiniEventSystem.OnStartGame += UpdateLevelText;

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
        }

        private void UpdateTimer(int time)
        {
            _timer.text = time.ToString();
        }

        private void UpdateLevelText()
        {
            _levelText.text = "Level " + (LevelManager.Instance.GetLevel + 1).ToString();
        }

        private void ActivateGameEndPanel(int gameEndID)
        {
            if(GameManager.Instance.IsGamePaused)
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

            
            GameManager.Instance.IsGamePaused = true;
        }

        private void SetInitialTextsOnScene()
        {
            _levelReachScore.text = GameManager.Instance.GetLevelData.LevelReachScore.ToString();
        }

        private void UpdateScoreUI(int newScore)
        {
            _score.text = newScore.ToString();
            _slider.value = (float)newScore / GameManager.Instance.GetLevelData.LevelReachScore;
        }

        public void NextLevelButton()
        {
            LevelManager.Instance.NextLevel();
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
