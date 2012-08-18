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
        /// <summary>
        /// the value [0.0, 1.0]
        /// dimension MUST have a value or a trace, the other MAY //TODO:
        /// </summary>
        protected float? value = null;
        /// <summary>
        /// confidence when we recognise the emotion [0.0, 1.0]
        /// </summary>
        protected float? confidence = null;
        /// <summary>
        /// trace if we want have a value over time
        /// </summary>
        protected Trace trace = null;

        /* ### CONSTRUCTORS ### */

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

        /* ### GETTER AND SETTER ### */

        public string Name
        {
            get { return name; }
            set
            {
                //TODO: check if name is in set and/or valid
                if (name != "")
                {
                    name = value;
                }
            }
        }

        public float? Value
        {
            get { return this.value; }
            set {
                if (value < 0 || value > 1)
                {
                    throw new EmotionMLException("only values in [0.0, 1.0] are allowed");
                }
                this.value = value; 
            }
        }

        public float? Confidence
        {
            get { return confidence; }
            set {
                if (value < 0 || value > 1)
                {
                    throw new EmotionMLException("only values in [0.0, 1.0] are allowed");
                }
                confidence = value; 
            }
        }

        public Trace Trace
        {
            get { return trace; }
            set { trace = value; }
        }
    }
}