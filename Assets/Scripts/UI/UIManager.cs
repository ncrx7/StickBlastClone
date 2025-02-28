using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _gameOverPanel;

        private void OnEnable()
        {
            MiniEventSystem.OnEndGame += ActivateGameOverPanel;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnEndGame -= ActivateGameOverPanel;
        }

        private void ActivateGameOverPanel()
        {
            _gameOverPanel.SetActive(true);
        }
    }
}
