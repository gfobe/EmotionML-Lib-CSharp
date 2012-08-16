using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class EmotionActionTendency : EmotionPart
    {
        /// <summary>
        /// Vocabularies for action tendencies of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string ACTIONTENDENCY_FRIJDA = "http://www.w3.org/TR/emotion-voc/xml#frijda-action-tendencies";

        public EmotionActionTendency(string name) : base(name)
        {}

        public EmotionActionTendency(string name, float? value) : base(name, value)
        {}

        public EmotionActionTendency(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}