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
    public class Dimension : Part
    {
        /// <summary>
        /// Vocabularies for dimensions of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string DIMENSION_PAD = "http://www.w3.org/TR/emotion-voc/xml#pad-dimensions";
        public const string DIMENSION_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-dimensions";
        public const string DIMENSION_INTENSITY = "http://www.w3.org/TR/emotion-voc/xml#intensity-dimension";


        public new double? Value
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

        public Dimension(string name, double? value) : base(name, value)
        {}

        public Dimension(string name, double? value, double? confidence) : base (name, value, confidence)
        {}       
    }
}