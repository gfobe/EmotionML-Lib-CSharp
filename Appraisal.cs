using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Appraisal : Part
    {
        /// <summary>
        /// Vocabularies for appraisals of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string APPRAISAL_OCC = "http://www.w3.org/TR/emotion-voc/xml#occ-appraisals";
        public const string APPRAISAL_SCHERER = "http://www.w3.org/TR/emotion-voc/xml#scherer-appraisals";
        public const string APPRAISAL_EMA = "http://www.w3.org/TR/emotion-voc/xml#ema-appraisals";

        public Appraisal(string name) : base(name)
        {}

        public Appraisal(string name, float? value) : base(name, value)
        {}

        public Appraisal(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}