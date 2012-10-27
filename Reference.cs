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
    public class Reference
    {
        const string EXPRESSED_BY = "expressedBy";
        const string EXPERIENCED_BY = "experiencedBy";
        const string TRIGGERED_BY = "triggeredBy";
        const string TARGETED_BY = "targetedBy";

        protected Uri uri = null;
        protected string role = null;
        protected string mediaType = null;

        public Reference(Uri uri) {
            this.uri = uri;
        }

        public Uri Uri
        {
            get { return uri; }
            set { uri = value; }

        }

        public string Role
        {
            get
            {
                if (role != null)
                {
                    return role;
                }
                else
                {
                    return EXPRESSED_BY;
                };
            }

            set {
                if (value == EXPRESSED_BY || value == EXPERIENCED_BY || value == TRIGGERED_BY || value == TARGETED_BY)
                {
                    role = value;
                }
                else
                {
                    throw new EmotionMLException(
                        "role must be one of " + EXPRESSED_BY + ", " + EXPERIENCED_BY + ", " + TRIGGERED_BY + ", " + TARGETED_BY
                    );
                }
            }
        }

        public string MediaType
        {
            get { return mediaType; }
            set {
                if (null == value || Helper.isMediaType(value))
                {
                    mediaType = value;
                }
                else
                {
                    throw new EmotionMLException('"' + value + "\" is no valid MIME type");
                }
            }
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

            Reference control = (Reference)obj;
            if (this.mediaType == control.MediaType
            && this.role == control.Role
            && this.uri.AbsoluteUri == control.Uri.AbsoluteUri)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// creates a DOM of reference
        /// </summary>
        /// <returns>XML DOM</returns>
        public XmlDocument ToDom()
        {
            XmlDocument reference = new XmlDocument();

            XmlElement referenceTag = reference.CreateElement("reference");
            referenceTag.Attributes.Append(createAttributeWithValue(reference, "uri", uri.ToString()));
            if (role != null)
            {
                referenceTag.Attributes.Append(createAttributeWithValue(reference, "role", role));
            }
            if (mediaType != null)
            {
                referenceTag.Attributes.Append(createAttributeWithValue(reference, "media-type", mediaType));
            }

            reference.AppendChild(referenceTag);

            return reference;
        }

        /// <summary>
        /// creates XML string of reference
        /// </summary>
        /// <returns>XML</returns>
        public string toXml()
        {
            return this.ToDom().OuterXml;
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
