using System.Collections;
using System.Collections.Generic;
using Enums;
using Sounds.Models;
using UnityEngine;

namespace Sounds.Controllers
{
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private List<Sound> _sounds;

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
            Sound sound = _sounds.Find(s => s.SoundType == soundType);
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


