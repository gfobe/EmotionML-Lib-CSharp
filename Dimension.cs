using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Dimension : Part
    {
        /// <summary>
        /// Vocabularies for dimensions of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string DIMENSION_PAD = "http://www.w3.org/TR/emotion-voc/xml#pad-dimensions";
        public const string DIMENSION_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-dimensions";
        public const string DIMENSION_INTENSITY = "http://www.w3.org/TR/emotion-voc/xml#intensity-dimension";


        public new float? Value
        {
            get { return this.value; }
            set
            {
                if (null == value)
                {
                    if(this.trace == null) {
                        throw new EmotionMLException("There has to be a value or a trace.");
                    }
                    this.value = value;
                    return;
                }
                else if (value < 0 || value > 1)
                {
                    throw new EmotionMLException("only values in [0.0, 1.0] are allowed");
                }
                this.trace = null; //only value or trace is allowed 
                this.value = value;
            }
        }

        public new Trace Trace
        {
            get { return trace; }
            set
            {
                if (null == value && this.value == null)
                {
                    throw new EmotionMLException("There has to be a value or a trace.");
                }
                //only value or trace is allowed
                this.value = null;
                this.trace = value;
            }
        }


        public Dimension(string name) : base(name)
        {}

        public Dimension(string name, float? value) : base(name, value)
        {}

        public Dimension(string name, float? value, float? confidence) : base (name, value, confidence)
        {}       
    }
}