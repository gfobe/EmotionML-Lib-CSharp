using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{

    public class EmotionML
    {
        /// <summary>
        /// Vocabularies for EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string CATEGORY_BIG6 = "http://www.w3.org/TR/emotion-voc/xml#big6";
        public const string CATEGORY_EVERYDAY = "http://www.w3.org/TR/emotion-voc/xml#everyday-categories";
        public const string CATEGORY_OCC = "http://www.w3.org/TR/emotion-voc/xml#occ-categories";
        public const string CATEGORY_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-categories";
        public const string CATEGORY_FRIJDA = "http://www.w3.org/TR/emotion-voc/xml#frijda-categories";
        public const string DIMENSION_PAD = "http://www.w3.org/TR/emotion-voc/xml#pad-dimensions";
        public const string DIMENSION_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-dimensions";
        public const string DIMENSION_INTENSITY = "http://www.w3.org/TR/emotion-voc/xml#intensity-dimension";
        public const string APPRAISAL_OCC = "http://www.w3.org/TR/emotion-voc/xml#occ-appraisals";
        public const string APPRAISAL_SCHERER = "http://www.w3.org/TR/emotion-voc/xml#scherer-appraisals";
        public const string APPRAISAL_EMA = "http://www.w3.org/TR/emotion-voc/xml#ema-appraisals";
        public const string ACTIONTENDENCY_FRIJDA = "http://www.w3.org/TR/emotion-voc/xml#frijda-action-tendencies";

        /// <summary>
        /// Namespace for EmotionML 1.0
        /// </summary>
        public const string NAMESPACE = "http://www.w3.org/2009/10/emotionml";

        /// <summary>
        /// XML-praefix of EmotionML
        /// </summary>
        public const string PRAEFIX = "emo";
    }
}