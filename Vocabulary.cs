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
        protected List<Item> items = new List<Item>();
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
                            Part.ACTIONTENDENCY + "."
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
        public void addItem(Item newItem) 
        {
            if (items.Exists(delegate(Item existingItem)
            {
                return existingItem.Name == newItem.Name;
            }) )
            {
                items.Add(newItem);
            }
            else
            {
                throw new EmotionMLException("item \"" + newItem.Name + "\" already exists.");
            }   
        }

        /// <summary>
        /// removes item from vokabulary
        /// </summary>
        /// <param name="name">name of item</param>
        public void removeItem(Item itemToRemove)
        {
            if (!items.Exists(delegate(Item existingItem){
                return itemToRemove.Name == existingItem.Name;
            }))
            {
                throw new EmotionMLException("item \"" + itemToRemove.Name + "\" is not in vocabulary.");
            }
            int index = items.FindIndex(delegate(Item existingItem) {
                return existingItem.Name == itemToRemove.Name;
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

            foreach (Item item in this.items)
            {
                vocabulary.AppendChild(item.ToDom());
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
