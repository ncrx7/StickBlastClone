using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

namespace Sounds.Models
{
    [Serializable]
    public class Sound
    {
        public SoundType SoundType;
        public AudioClip Clip;
    }
}
