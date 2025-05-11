using System.Collections;
using System.Collections.Generic;
using EntitiesData.Levels;
using Mainpanel;
using UnityUtils.BaseClasses;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private LevelManager _levelManager;

    [SerializeField] private LevelData _levelData;
    [SerializeField] private int _score;
    [SerializeField] private int _scoreIncreaseAmountPerCellDestroy;

    public bool IsGamePaused { get; set; } = false;

    [Inject]
    private void InitializeDependencies(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    private void OnEnable()
    {
        MiniEventSystem.IncreaseScore += HandleIncreaseScore;
    }

    private void OnDisable()
    {
        MiniEventSystem.IncreaseScore -= HandleIncreaseScore;
    }

    protected void Awake()
    {
        _levelData = _levelManager.GetCurrentLevelData();
    }

    private void Start()
    {
        MiniEventSystem.OnStartGame?.Invoke();
        Application.targetFrameRate = 120;
    }

    public void SetLevelData(LevelData levelData)
    {
        _levelData = levelData;
    }

    private void HandleIncreaseScore(int newScore)
    {
        _score = newScore;

        if (_score >= _levelData.LevelReachScore)
        {
            MiniEventSystem.OnEndGame?.Invoke(2);
            //Debug.Log("GAME END SUCESSS");
        }
    }

    public int GetScore => _score;
    public int GetScoreIncreaseAmountPerCellDestroy => _scoreIncreaseAmountPerCellDestroy;
    public LevelData GetLevelData => _levelData;
}
