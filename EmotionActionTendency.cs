using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionActionTendency : EmotionPart
    {
        public EmotionActionTendency(string name) : base(name)
        {}

        public EmotionActionTendency(string name, float? value) : base(name, value)
        {}

        public EmotionActionTendency(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}