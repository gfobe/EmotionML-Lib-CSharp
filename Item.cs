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
                /*if (Helper.isNmtoken(value))
                {
                    name = value;
                }
                else
                {
                    throw new EmotionMLException("Name of item have to be a xsd:nmtoken.");
                }*/
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


        public bool Equals(object obj, bool ignoreInfoPart = false)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false; //wrong type
            }
            if (base.Equals(obj))
            {
                return true; //same instance
            }

            Item control = (Item)obj;
            if (!ignoreInfoPart)
            {
                if (!this.info.Equals(control.Info))
                {
                    return false;
                }
            }

            return (this.name == control.Name);
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
                XmlNode importedNode = item.ImportNode(info.ToDom().FirstChild, true);
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
            return ToDom().OuterXml;
        }
    }
}
