using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EntitiesData.Levels;
using UnityUtils.BaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using Data.Controllers;
using Zenject;
using Data.Model;

namespace Mainpanel
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelData> _levelDataList;
        [SerializeField] private int _currentLevel;

        private GameDataHandler _gameDataHandler;

        [Inject]
        private void InitializeDependencies(GameDataHandler gameDataHandler)
        {
            _gameDataHandler = gameDataHandler;
        }

        private void OnEnable()
        {
            MiniEventSystem.OnCompleteGameDataLoad += InitLevelData;
        }

        private void OnDisable()
        {
            MiniEventSystem.OnCompleteGameDataLoad -= InitLevelData;
        }

        private void InitLevelData(GameData gameData)
        {
            _currentLevel = gameData.UserLevel;
            Debug.Log("current level -> " + _currentLevel);
        }

        public LevelData GetCurrentLevelData()
        {
            return _levelDataList[_currentLevel - 1];
        }

        public void NextLevel()
        {
            _currentLevel++;

            if(_currentLevel % (_levelDataList.Count + 1) == 0 )
                _currentLevel = 1;

            _gameDataHandler.GetGameDataObjectReference().UserLevel = _currentLevel;

            _gameDataHandler.UpdateGameDataFile();
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
        public List<LevelData> GetAllLevelData => _levelDataList;
    }
}
