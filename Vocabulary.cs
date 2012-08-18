using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Vocabulary
    {
        /// <summary>
        /// type of vocabulary (category, dimension, appraisal, action-tendency)
        /// </summary>
        protected string type;
        /// <summary>
        /// xml:ID of vocabulary
        /// </summary>
        protected string id;
        /// <summary>
        /// items in vocabulary
        /// </summary>
        protected List<string> items = new List<string>(); //TODO: extra Klasse, da ein Info-Block rein muss.
        /// <summary>
        /// <info/> for vocabulary
        /// </summary>
        protected Info info = null;

        public Vocabulary(string type, string id) 
        {
            Id = id;
            Type = type;
        }

        public string Id
        {
            get { return id; }
            set
            {
                //TODO: test of valid attribute string
                if (value == "")
                {
                    throw new EmotionMLException("Emotion vocabulary must have an id.");
                }

                id = value;
            }
        }

        public string Type
        {
            get { return type; }
            set
            {
                switch (value)
                {
                    case Part.CATEGORY:
                    case Part.DIMENSION:
                    case Part.APPRAISAL:
                    case Part.ACTIONTENDENCY:
                        break;
                    default:
                        throw new EmotionMLException(
                            "Emotion vocabulary must have a type like " +
                            Part.CATEGORY + ", " +
                            Part.DIMENSION + ", " +
                            Part.APPRAISAL + ", " +
                            Part.ACTIONTENDENCY
                        );
                }

                type = value;
            }

        }
             
        public Info Info {
            get { return info; }
            set { info = value; }
        }

        /// <summary>
        /// adds item to vokabulary
        /// </summary>
        /// <param name="name">name of item</param>
        public void addItem(string name) 
        {
            //TODO: test of valid attribute string
            items.Add(name);
        }

        /// <summary>
        /// removes item from vokabulary
        /// </summary>
        /// <param name="name">name of item</param>
        public void removeItem(string name)
        {
            if (!items.Contains(name))
            {
                throw new EmotionMLException("item \"" + name + "\" is not in vocabulary");
            }
            int index = items.FindIndex(delegate(string itemname) {
                return itemname == name;
            });
            items.RemoveAt(index);
        }

        /// <summary>
        /// creates a DOM of Emotion
        /// </summary>
        /// <returns>DOM of emotion definition</returns>
        public XmlDocument ToDom()
        {
            XmlDocument vocabularyXml = new XmlDocument();

            //set up <vocabulary>
            XmlElement vocabulary = vocabularyXml.CreateElement("vocabulary");
            vocabulary.SetAttribute("type", this.type);
            vocabulary.SetAttribute("id", this.id);
            if (info != null)
            {
                vocabulary.AppendChild(info.ToDom());
            }

            foreach (string entry in this.items)
            {
                XmlElement item = vocabularyXml.CreateElement("item");
                item.SetAttribute("name", entry);
                vocabulary.AppendChild(item);
            }

            vocabularyXml.AppendChild(vocabulary);
            return vocabularyXml;
        }

        /// <summary>
        /// generates the XML for vokabulary
        /// </summary>
        /// <returns>XML</returns>
        public string ToXml()
        {
            return ToDom().ToString();
        }
    }
}
