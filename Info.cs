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
