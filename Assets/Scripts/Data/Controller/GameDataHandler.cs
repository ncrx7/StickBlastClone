using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data.Model;
using Shapes;
using UnityEngine;
using UnityUtils.Core.DataManagment;

namespace Data.Controllers
{
    public class GameDataHandler : MonoBehaviour
    {
        [SerializeField] private string _jsonFileName;
        [SerializeField] GameData _gameData;
        private DataWriterAndReader<GameData> _dataWriterAndReader;
        public bool IsDataLoadFinished = false;

        private void Awake()
        {
            _dataWriterAndReader = new DataWriterAndReader<GameData>(Application.persistentDataPath, _jsonFileName);
        }

        private void Start()
        {
            InitializeData();
        }

        private async void InitializeData()
        {
            await LoadGameDataFile();

            await UniTask.Delay(1000); //fake waiting
           
            IsDataLoadFinished = true;
            MiniEventSystem.OnCompleteGameDataLoad?.Invoke(_gameData);
        }

        private async UniTask LoadGameDataFile()
        {
            _gameData = await _dataWriterAndReader.InitializeDataFile(CreateNewGameDataObject);
        }

        public void UpdateGameDataFile()
        {
            _dataWriterAndReader.UpdateDataFile(_gameData);
        }

        public GameData CreateNewGameDataObject()
        {
            GameData gameData = new GameData("Garawell Games", 1);
            return gameData;
        }

        public GameData GetGameDataObjectReference()
        {
            return _gameData;
        }

        public void SetPlayerDataObjectReference(GameData gameData)
        {
            _gameData = gameData;
        }
    }
}
