using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionCategory : EmotionPart
    {
        public EmotionCategory(string name) : base(name)
        {}

        public EmotionCategory(string name, float? value) : base(name, value)
        {}

        public EmotionCategory(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}