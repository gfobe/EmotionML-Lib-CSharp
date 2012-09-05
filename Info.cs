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
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Info
    {
        /// <summary>
        /// content of info area
        /// </summary>
        XmlNodeList content = null;

        /// <summary>
        /// id for this info area
        /// </summary>
        string id = null;

        /// <summary>
        /// plaintext for <info/>
        /// </summary>
        string plaintext = null;

        public Info()
        {
            content = null;
        }

        public XmlNodeList Content
        {
            get { return content; }
            set { content = value; }
        }

        public string Id
        {
            get { return id; }
            set {
                if (Helper.isXsdId(value))
                {
                    id = value;
                }
                else
                {
                    throw new EmotionMLException("Id have to be a xsd:ID.");
                }
            }
        }

        public string Plaintext
        {
            get { return plaintext; }
            set { plaintext = value; }
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

            Info control = (Info)obj;
            if (this.content.ToString() == control.Content.ToString()
            && this.id == control.Id
            && this.plaintext == control.Plaintext)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// creates a DOM of <info/>
        /// </summary>
        /// <returns>XML DOM</returns>
        public XmlDocument ToDom()
        {
            XmlDocument info = new XmlDocument();

            XmlElement infoTag = info.CreateElement("info");
            if (id != null)
            {
                XmlAttribute idAttribute = info.CreateAttribute("id");
                idAttribute.Value = id;
                infoTag.AppendChild(idAttribute);
            }
            foreach (XmlNode infoContent in this.content)
            {
                XmlNode importedNode = info.ImportNode(infoContent, true);
                infoTag.AppendChild(importedNode);
            }
            if (plaintext != null)
            {
                infoTag.AppendChild(info.CreateTextNode(plaintext));
            }

            info.AppendChild(infoTag);

            return info;
        }

        /// <summary>
        /// creates XML of info area
        /// </summary>
        /// <returns>XML within <info/> tag</returns>
        public string ToXml() 
        {
            return this.ToDom().OuterXml;
        }

    }
}
