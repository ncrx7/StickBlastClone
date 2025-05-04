using System.Collections;
using System.Collections.Generic;
using DataModel;
using Enums;
using Sounds.Models;
using UnityEngine;
using Zenject;

namespace Sounds.Controllers
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        private GameSettings _gameSettings;

        [Inject]
        private void InitializeDependencies(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        private void OnEnable()
        {
            MiniEventSystem.PlaySoundClip += HanlePlaySoundClip;
        }

        private void OnDisable()
        {
            MiniEventSystem.PlaySoundClip -= HanlePlaySoundClip;
        }

        private void HanlePlaySoundClip(SoundType soundType)
        {
            Sound sound = _gameSettings.Sounds.Find(s => s.SoundType == soundType);
            if (sound != null && sound.Clip != null)
            {
                _audioSource.PlayOneShot(sound.Clip);
            }
            else
            {
                Debug.LogWarning("Undefined sound type!!");
            }
        }
    }
}


