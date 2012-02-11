using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionAppraisal : EmotionPart
    {
        public EmotionAppraisal(string name) : base(name)
        {}

        public EmotionAppraisal(string name, float? value) : base(name, value)
        {}

        public EmotionAppraisal(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}