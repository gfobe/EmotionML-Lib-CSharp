using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Item
    {
        /// <summary>
        /// name (attribute) of the item
        /// </summary>
        protected string name = null;
        /// <summary>
        /// <info/> section for item
        /// </summary>
        protected Info info = null;


        public string Name
        {
            get { return name;  }
            set
            {
                //TODO: validate
                name = value;
            }
        }

        public Info Info
        {
            get { return info; }
            set { info = value; }
        }

        public Item(string name)
        {
            Name = name;
        }


        /// <summary>
        /// creates a DOM of item
        /// </summary>
        /// <returns>XML DOM</returns>
        public XmlDocument ToDom()
        {
            XmlDocument item = new XmlDocument();

            XmlAttribute attribute = item.CreateAttribute("name");
            attribute.Value = name;
            XmlElement itemTag = item.CreateElement("item");
            itemTag.AppendChild(attribute);

            item.AppendChild(itemTag);
            if (info != null)
            {
                itemTag.AppendChild(info.ToDom());
            }

            return item;
        }

        /// <summary>
        /// creates XML string of item
        /// </summary>
        /// <returns>XML</returns>
        public string ToXml()
        {
            return ToDom().ToString();
        }
    }
}
