using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mainpanel;
using UnityEngine;

namespace Mainpanel
{
    public class UIManager : MonoBehaviour
    {
        public void StartTheLevelButton()
        {
            LevelManager.Instance.LoadSceneAsync(1).Forget();
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}
