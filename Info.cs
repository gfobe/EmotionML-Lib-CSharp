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
        XmlNode content = null;

        /// <summary>
        /// id for this info area
        /// </summary>
        string id = null;

        public Info()
        {
            content = null;
        }

        public XmlNode Content {
            get { return content; }
            set { content = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
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

            infoTag.AppendChild(this.content);
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
