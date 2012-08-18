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
                //TODO: validate http://www.w3.org/TR/1999/REC-xml-names-19990114/#NT-NCName
                id = value; 
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
            //TODO: set namespace?
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
                infoTag.AppendChild(infoContent);
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
            return this.ToDom().ToString();
        }

    }
}
