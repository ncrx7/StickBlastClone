using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;
using Zenject;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int _time;
    CancellationTokenSource _cts;

    private GameManager _gameManager;

    [Inject]
    private void InitializeDependencies(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _time = _gameManager.GetLevelData.Timer;

        _cts = new CancellationTokenSource();

        StartNewCountdown(_time);
    }

    private CancellationTokenSource _cancellationTokenSource;

    private void OnDestroy()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void StartNewCountdown(int duration)
    {
        _cancellationTokenSource?.Cancel();

        _cancellationTokenSource = new CancellationTokenSource();

        StartCountdown(duration, _cancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid StartCountdown(int duration, CancellationToken cancellationToken)
    {
        await UniTask.WaitUntil(() => _gameManager.IsGamePaused == false);

        while (duration >= 0 && !_gameManager.IsGamePaused)
        {
            if (cancellationToken.IsCancellationRequested)
                return; 

            MiniEventSystem.OnTimerWork?.Invoke(duration);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.Timer);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
            duration--;
        }

        MiniEventSystem.OnEndGame?.Invoke(1);
    }
}
