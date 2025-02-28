using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private GameObject _gameSuccessPanel;
        [SerializeField] private TextMeshProUGUI _levelReachScore;
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private Slider _slider;

        

        private void OnEnable()
        {
            MiniEventSystem.OnStartGame += SetInitialTextsOnScene;
            MiniEventSystem.OnEndGame += ActivateGameOverPanel;
             MiniEventSystem.OnEndGame += ActivateGameSuccesPanel;
            MiniEventSystem.IncreaseScore += UpdateScoreUI;
            
            UpdateScoreUI(0);
        }

        private void OnDisable()
        {
            MiniEventSystem.OnEndGame -= ActivateGameOverPanel;
             MiniEventSystem.OnEndGame -= ActivateGameSuccesPanel;
            MiniEventSystem.OnStartGame -= SetInitialTextsOnScene;
            MiniEventSystem.IncreaseScore -= UpdateScoreUI;
        }

        private void ActivateGameOverPanel(bool success)
        {
            if(success)
               return;

            _gameOverPanel.SetActive(true);
        }

        private void ActivateGameSuccesPanel(bool success)
        {
            if(!success)
                return;

            _gameSuccessPanel.SetActive(true);
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
    }
}
