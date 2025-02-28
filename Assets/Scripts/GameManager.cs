using System.Collections;
using System.Collections.Generic;
using EntitiesData.Levels;
using MyUtils.Base;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    [SerializeField] private LevelData _levelData;
    [SerializeField] private int _score;
    [SerializeField] private int _scoreIncreaseAmountPerCellDestroy;

    private void OnEnable()
    {
        MiniEventSystem.IncreaseScore += HandleIncreaseScore;
    }

    private void OnDisable()
    {
        MiniEventSystem.IncreaseScore -= HandleIncreaseScore;
    }

    private void Start()
    {
        MiniEventSystem.OnStartGame?.Invoke();
    }

    private void HandleIncreaseScore(int newScore)
    {
        _score = newScore;

        if(_score >= _levelData.LevelReachScore)
        {
            MiniEventSystem.OnEndGame?.Invoke(true);
            Debug.Log("GAME END SUCESSS");
        }
    }

    public int GetScore => _score;
    public int GetScoreIncreaseAmountPerCellDestroy => _scoreIncreaseAmountPerCellDestroy;
    public LevelData GetLevelData => _levelData;
}
