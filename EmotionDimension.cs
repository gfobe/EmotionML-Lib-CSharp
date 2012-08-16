using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class EmotionDimension : EmotionPart
    {
        /// <summary>
        /// Vocabularies for dimensions of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string DIMENSION_PAD = "http://www.w3.org/TR/emotion-voc/xml#pad-dimensions";
        public const string DIMENSION_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-dimensions";
        public const string DIMENSION_INTENSITY = "http://www.w3.org/TR/emotion-voc/xml#intensity-dimension";

        public EmotionDimension(string name) : base(name)
        {}

        public EmotionDimension(string name, float? value) : base(name, value)
        {}

        public EmotionDimension(string name, float? value, float? confidence) : base (name, value, confidence)
        {}       
    }
}