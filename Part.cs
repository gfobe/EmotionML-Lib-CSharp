using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public abstract class Part
    {
        /// <summary>
        /// possible parts of an emotion annotation
        /// </summary>
        public const string CATEGORY = "category";
        public const string DIMENSION = "dimension";
        public const string APPRAISAL = "appraisal";
        public const string ACTIONTENDENCY = "actiontendency";


        public string name;
        public float? confidence = null;
        //TODO: dimension MUST have a value or a trace, the other MAY
        public float? value = null;
        public Trace trace = null;

        public Part(string name)
        {
            this.name = name;
        }

        //OPTIMIZE: shorter with :this(name)
        public Part(string name, float? value)
        {
            this.name = name;
            this.value = value;
        }

        public Part(string name, float? value, float? confidence)
        {
            this.name = name;
            this.value = value;
            this.confidence = confidence;
        }

    }
}