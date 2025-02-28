using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Enums;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int _time;

    private void Start()
    {
        _time = GameManager.Instance.GetLevelData.Timer;

        StartCountdown(_time).Forget();
    }

    public async UniTaskVoid StartCountdown(int duration)
    {
        while (duration >= 0)
        {
            MiniEventSystem.OnTimerWork?.Invoke(duration);
            MiniEventSystem.PlaySoundClip?.Invoke(SoundType.Timer);

            await UniTask.Delay(TimeSpan.FromSeconds(1));
            duration--;
        }

        MiniEventSystem.OnEndGame?.Invoke(false);
    }
}
