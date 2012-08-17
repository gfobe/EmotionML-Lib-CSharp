using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    class EmotionList : List<Emotion>
    {
        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        public Set<Category> category = null;
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        public Set<Dimension> dimension = null;
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        public Set<Appraisal> appraisal = null;
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        public Set<ActionTendency> actionTendency = null;

        /// <summary>
        /// emotion vacabularies
        /// </summary>
        protected List<Vocabulary> vocabularies = null;

        /// <summary>
        /// adds a emotion vocabulary to list
        /// </summary>
        /// <param name="vocabulary">defined emotion vocabulary</param>
        public void addVocabulary(Vocabulary vocabulary)
        {
            this.vocabularies.Add(vocabulary);
        }

        /// <summary>
        /// creates a DOM-list of emotions in list
        /// </summary>
        /// <returns>DOM of emotionml notation</returns>
        public XmlDocument ToDom()
        {
            //init root node with attributes
            XmlDocument emotionmlXml = new XmlDocument();
            XmlElement emotionml = emotionmlXml.CreateElement("emotionml");

            if (this.category != null)
            {
                emotionml.SetAttribute("category-set", this.category.Uri.AbsoluteUri);
            }
            if (this.dimension != null)
            {
                emotionml.SetAttribute("dimension-set", this.dimension.Uri.AbsoluteUri);
            }
            if (this.appraisal != null)
            {
                emotionml.SetAttribute("appraisal-set", this.appraisal.Uri.AbsoluteUri);
            }
            if (this.actionTendency != null)
            {
                emotionml.SetAttribute("action-tendency-set", this.actionTendency.Uri.AbsoluteUri);
            }

            //add vocabularies to list
            foreach (Vocabulary vocabulary in this.vocabularies)
            {
                XmlDocument vocabularyDom = vocabulary.ToDom();
                emotionml.AppendChild(vocabularyDom);
            }

            //add emotions to list
            for (int i = 0; i < this.Count; i++)
            {
                XmlDocument emotion = this.ElementAt(i).ToDom();
                emotionml.AppendChild(emotion);
            }

            emotionmlXml.AppendChild(emotionml);

            return emotionmlXml;
        }

        /// <summary>
        /// creates EmotionML-XML for emotion list
        /// </summary>
        /// <returns>XML of emotions</returns>
        public string ToXml()
        {
            return this.ToDom().ToString();
        }
    }
}
