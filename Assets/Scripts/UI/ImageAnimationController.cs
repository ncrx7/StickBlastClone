using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ImageAnimationController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image targetImage;

        [Header("Animation Settings")]
        [SerializeField] private Sprite[] animationFrames;
        [SerializeField] private float frameRate = 0.1f; // saniye cinsinden (örneğin: 0.1s = 10 FPS)

        private CancellationTokenSource _cts;

        private void Start()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            // Önceki animasyon varsa iptal et
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            // Animasyonu başlat
            AnimateAsync(_cts.Token).Forget();
        }

        public void StopAnimation()
        {
            _cts?.Cancel();
        }

        private async UniTaskVoid AnimateAsync(CancellationToken token)
        {
            int frameCount = animationFrames.Length;
            int currentIndex = 0;

            while (!token.IsCancellationRequested)
            {
                targetImage.sprite = animationFrames[currentIndex];
                currentIndex = (currentIndex + 1) % frameCount;

                await UniTask.Delay(TimeSpan.FromSeconds(frameRate), cancellationToken: token);
            }
        }
    }
}
