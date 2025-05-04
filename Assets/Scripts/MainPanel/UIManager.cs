using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mainpanel;
using UnityEngine;
using Zenject;

namespace Mainpanel
{
    public class UIManager : MonoBehaviour
    {
        private LevelManager _levelManager;

        [Inject]
        private void InitializeDependencies(LevelManager levelManager)
        {
            _levelManager = levelManager;
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
