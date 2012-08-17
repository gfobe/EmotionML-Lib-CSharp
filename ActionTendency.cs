using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class ActionTendency : Part
    {
        /// <summary>
        /// Vocabularies for action tendencies of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string ACTIONTENDENCY_FRIJDA = "http://www.w3.org/TR/emotion-voc/xml#frijda-action-tendencies";

        public ActionTendency(string name) : base(name)
        {}

        public ActionTendency(string name, float? value) : base(name, value)
        {}

        public ActionTendency(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}