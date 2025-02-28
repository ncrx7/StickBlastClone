using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int _time;
    CancellationTokenSource _cts;

    private void Start()
    {
        _time = GameManager.Instance.GetLevelData.Timer;

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
        await UniTask.WaitUntil(() => GameManager.Instance.IsGamePaused == false);

        while (duration >= 0 && !GameManager.Instance.IsGamePaused)
        {
            if (cancellationToken.IsCancellationRequested)
                return; 

            MiniEventSystem.OnTimerWork?.Invoke(duration);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.Timer);

            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
            duration--;
        }

        MiniEventSystem.OnEndGame?.Invoke(false);
    }
}
