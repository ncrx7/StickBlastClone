using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitiesData.Levels;
using MyUtils.Base;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mainpanel
{
    public class LevelManager : SingletonBehavior<LevelManager>
    {
        [SerializeField] private List<LevelData> _levelDataList;
        [SerializeField] private int _currentLevel = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public LevelData GetCurrentLevelData()
        {
            return _levelDataList[_currentLevel];
        }

        public void NextLevel()
        {
            _currentLevel++;
            _currentLevel %= _levelDataList.Count;
        }

        public async UniTask LoadSceneAsync(int sceneIndex)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    await UniTask.Delay(1000);
                    asyncOperation.allowSceneActivation = true;
                }

                await UniTask.Yield();
            }
        }

        public int GetLevel => _currentLevel;
    }
}
