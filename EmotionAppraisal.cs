using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionAppraisal : EmotionPart
    {
        /// <summary>
        /// Vocabularies for appraisals of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string APPRAISAL_OCC = "http://www.w3.org/TR/emotion-voc/xml#occ-appraisals";
        public const string APPRAISAL_SCHERER = "http://www.w3.org/TR/emotion-voc/xml#scherer-appraisals";
        public const string APPRAISAL_EMA = "http://www.w3.org/TR/emotion-voc/xml#ema-appraisals";

        public EmotionAppraisal(string name) : base(name)
        {}

        public EmotionAppraisal(string name, float? value) : base(name, value)
        {}

        public EmotionAppraisal(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}