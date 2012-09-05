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
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Trace
    {
        /// <summary>
        /// sampling freuquency in Hz
        /// </summary>
        protected double frequency;
        /// <summary>
        /// numeric scale values from interval [0;1] (changes over time)
        /// space seperated
        /// </summary>
        protected string samples = "";


        public Trace(double frequency, string samples) {
            this.Frequency = frequency;
            this.Samples = samples;
        }

        public Trace(string frequencyString, string samples)
        {
            this.setFrequency(frequencyString);
            this.Samples = samples;
        }

        public double Frequency {
            get { return frequency; }
            set
            {
                if (value < 0)
                {
                    throw new EmotionMLException("frequency must have a positive value");
                }
               
                this.frequency = value;
            }

        }

        public string Samples
        {
            get { return samples; }
            set
            {
                if (value == "")
                {
                    throw new EmotionMLException("samples needed");
                }

                //do samples have right format (space seperated values within [0.0, 1.0])
                Regex samplesRegEx = new Regex("^(1\\.0|0\\.\\d+)( (1\\.0|0\\.\\d+))*$");

                if (!samplesRegEx.IsMatch(value))
                {
                    throw new EmotionMLException("illegal format of samples");
                }

                this.samples = value;
            }
        }

        /// <summary>
        /// set frequency by frequency-string (within Hz)
        /// </summary>
        /// <param name="frequency">frequency string</param>
        public void setFrequency(string frequency) {
            //floating point (or integer) + optional whitespace + Hz
            Regex frequencyRegEx = new Regex("^(\\d+(\\.\\d+)?)\\s?Hz$");
            MatchCollection matches = frequencyRegEx.Matches(frequency);

            if (matches.Count == 0)
            {
                throw new EmotionMLException("illegal format of frequency string");
            }

            this.Frequency = double.Parse(matches.GetEnumerator().Current.ToString());
        }

        /// <summary>
        /// returns frequency as string with Hz
        /// </summary>
        /// <returns>frequency string (with Hz)</returns>
        public string getFrequency()
        {
            return frequency.ToString() + "Hz";
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false; //wrong type
            }
            if (base.Equals(obj))
            {
                return true; //same instance
            }

            Trace control = (Trace)obj;
            if (this.frequency == control.Frequency
            && this.samples == control.Samples)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// creates a DOM of trace
        /// </summary>
        /// <returns>XML DOM</returns>
        public XmlDocument ToDom()
        {
            XmlDocument trace = new XmlDocument();

            XmlElement traceTag = trace.CreateElement("trace");
            traceTag.AppendChild(createAttributeWithValue(trace, "freq", getFrequency()));
            traceTag.AppendChild(createAttributeWithValue(trace, "samples", samples));
            trace.AppendChild(traceTag);

            return trace;
        }

        /// <summary>
        /// creates XML string of trace
        /// </summary>
        /// <returns>XML</returns>
        public string toXml()
        {
            return ToDom().OuterXml;
        }


        /// <summary>
        /// creates an XmlAttribute with value in context of XmlDocument
        /// </summary>
        /// <param name="parentDocument">parent XML document</param>
        /// <param name="attributeName">name of attribute</param>
        /// <param name="value">value of attribute</param>
        /// <returns></returns>
        private XmlAttribute createAttributeWithValue(XmlDocument parentDocument, string attributeName, string value)
        {
            XmlAttribute attribute = parentDocument.CreateAttribute(attributeName);
            attribute.Value = value;
            return attribute;
        }
    }
}
