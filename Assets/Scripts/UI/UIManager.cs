using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _gameSuccessPanel;
        [SerializeField] private TextMeshProUGUI _levelReachScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private Slider _slider;



        private void OnEnable()
        {
            MiniEventSystem.OnStartGame += SetInitialTextsOnScene;
            MiniEventSystem.OnEndGame += ActivateGameOverPanel;
            MiniEventSystem.OnEndGame += ActivateGameSuccesPanel;
            MiniEventSystem.IncreaseScore += UpdateScoreUI;
            MiniEventSystem.ActivateLoadingUI += HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI += DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork += UpdateTimer;

            UpdateScoreUI(0);
        }

        private void OnDisable()
        {
            MiniEventSystem.OnEndGame -= ActivateGameOverPanel;
            MiniEventSystem.OnEndGame -= ActivateGameSuccesPanel;
            MiniEventSystem.OnStartGame -= SetInitialTextsOnScene;
            MiniEventSystem.IncreaseScore -= UpdateScoreUI;
            MiniEventSystem.ActivateLoadingUI -= HandleActivaiateLoadingUI;
            MiniEventSystem.DeactivateLoadingUI -= DeActivaiteLoadingUI;
            MiniEventSystem.OnTimerWork -= UpdateTimer;
        }

        private void UpdateTimer(int time)
        {
            _timer.text = time.ToString();
        }

        private void ActivateGameOverPanel(bool success)
        {
            if (success)
                return;

            _gameOverPanel.SetActive(true);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.End);
            GameManager.Instance.IsGamePaused = true;
        }

        private void ActivateGameSuccesPanel(bool success)
        {
            if (!success)
                return;

            _gameSuccessPanel.SetActive(true);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.SucessEnd);
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

        public void ResetSceneButton()
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
