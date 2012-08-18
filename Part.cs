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

        /// <summary>
        /// name of the entry (name out of set)
        /// </summary>
        protected string name;
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


        public bool Equals(object obj, bool ignoreConfidencePart = false)  //OPTIMIZE: one interface for ignoring (confidence, info)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false; //wrong type
            }
            if (base.Equals(obj))
            {
                return true; //same instance
            }

            Part control = (Part)obj;
            if (!ignoreConfidencePart)
            {
                if (this.confidence == control.Confidence)
                {
                    return false;
                }
            }

            if (this.name == control.Name
            && this.value == control.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}