using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    class EmotionVocabulary
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
        protected List<string> items;

        public EmotionVocabulary(string type, string id) 
        {
            //TODO: test of valid attribute string
            if (id == "")
            {
                throw new EmotionException("Emotion vocabulary must have an id.");
            }
            switch(type) {
                case EmotionPart.CATEGORY:
                case EmotionPart.DIMENSION:
                case EmotionPart.APPRAISAL:
                case EmotionPart.ACTIONTENDENCY:
                    break;
                default:
                    throw new EmotionException(
                        "Emotion vocabulary must have a type like " + 
                        EmotionPart.CATEGORY + ", " + 
                        EmotionPart.DIMENSION + ", " +
                        EmotionPart.APPRAISAL + ", " +
                        EmotionPart.ACTIONTENDENCY
                    );
            }

            this.id = id;
            this.type = type;
        }

        /// <summary>
        /// adds item to list
        /// </summary>
        /// <param name="name">name of item</param>
        public void addItem(string name) 
        {
            //TODO: test of valid attribute string
            items.Add(name);
        }

        /// <summary>
        /// creates a DOM of Emotion
        /// </summary>
        /// <returns>DOM of emotion definition</returns>
        public XmlDocument ToDom()
        {
            XmlDocument vocabularyXml = new XmlDocument();

            //set up <vacabulary>
            XmlElement vocabulary = vocabularyXml.CreateElement("vocabulary");
            vocabulary.SetAttribute("type", this.type);
            vocabulary.SetAttribute("id", this.id);           

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
        /// generates the XML EmotionML
        /// </summary>
        /// <returns>XML representation of emotion</returns>
        public string ToXml()
        {
            return ToDom().ToString();
        }
    }
}
