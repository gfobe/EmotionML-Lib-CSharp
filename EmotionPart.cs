using System;
using System.Collections.Generic;
using System.Linq;

namespace EmotionML
{
    public class EmotionPart
    {
        /// <summary>
        /// possible parts of an emotion annotation
        /// </summary>
        public const string CATEGORY = "category";
        public const string DIMENSION = "dimension";
        public const string APPRAISAL = "appraisal";
        public const string ACTIONTENDENCY = "actiontendency";


        public string name;
        public float? value = null;
        public float? confidence = null;

        public EmotionPart(string name)
        {
            this.name = name;
        }

        public EmotionPart(string name, float? value)
        {
            this.name = name;
            this.value = value;
        }

        public EmotionPart(string name, float? value, float? confidence)
        {
            this.name = name;
            this.value = value;
            this.confidence = confidence;
        }

    }
}