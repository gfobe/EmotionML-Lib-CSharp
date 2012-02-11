using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionDimension : EmotionPart
    {
        public EmotionDimension(string name) : base(name)
        {}

        public EmotionDimension(string name, float? value) : base(name, value)
        {}

        public EmotionDimension(string name, float? value, float? confidence) : base (name, value, confidence)
        {}       
    }
}