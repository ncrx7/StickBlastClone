using System.Collections;
using System.Collections.Generic;
using Enums;
using Shapes;
using UnityEngine;

namespace SignalEvents
{
    public class ShapePlacedSignal
    {
        public ShapeManager shapeManager;

        public ShapePlacedSignal(ShapeManager shapeManager)
        {
            this.shapeManager = shapeManager;
        }
    }

    public class PlaySoundClipSignal
    {
        public SoundType soundType;

        public PlaySoundClipSignal(SoundType soundType)
        {
            this.soundType = soundType;
        }
    }
}
