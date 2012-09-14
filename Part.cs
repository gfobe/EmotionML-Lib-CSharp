// -- FreeBSD License ---------------------------------------------------------
// Copyright (c) 2012, Gerhard Fobe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
// 2. Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER 
// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// ----------------------------------------------------------------------------

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
        protected double? value = null;
        /// <summary>
        /// confidence when we recognise the emotion [0.0, 1.0]
        /// </summary>
        protected double? confidence = null;
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
        public Part(string name, double? value)
        {
            this.name = name;
            this.value = value;
        }

        public Part(string name, double? value, double? confidence)
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

        public double? Value
        {
            get { return this.value; }
            set {
                if (null == value)
                {
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

        public double? Confidence
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
            set 
            { 
                //only value or trace is allowed
                this.value = null;
                this.trace = value; 
            }
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