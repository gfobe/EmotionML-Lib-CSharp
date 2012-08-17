using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    class Reference
    {
        const string EXPRESSED_BY = "expressedBy";
        const string EXPERIENCED_BY = "experiencedBy";
        const string TRIGGERED_BY = "triggeredBy";
        const string TARGETED_BY = "targetedBy";

        protected Uri uri = null;
        protected string role = null;
        protected string mediaType = null;  //TODO: validate media type

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
            set { mediaType = value; }
        }

        /// <summary>
        /// creates a DOM of reference
        /// </summary>
        /// <returns>XML DOM</returns>
        public XmlDocument ToDom()
        {
            XmlDocument reference = new XmlDocument();

            XmlElement referenceTag = reference.CreateElement("reference");
            referenceTag.AppendChild(createAttributeWithValue(reference, "uri", uri.ToString()));
            if (role != null)
            {
                referenceTag.AppendChild(createAttributeWithValue(reference, "role", role));
            }
            if (mediaType != null)
            {
                referenceTag.AppendChild(createAttributeWithValue(reference, "media-type", mediaType));
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
            return this.ToDom().ToString();
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
