using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EmotionML
{
    class EmotionList : List<Emotion>
    {
        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        public EmotionSet<EmotionCategory> category = null;
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        public EmotionSet<EmotionDimension> dimension = null;
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        public EmotionSet<EmotionAppraisal> appraisal = null;
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        EmotionSet<EmotionActionTendency> actionTendency = null;

        /// <summary>
        /// emotion vacabularies
        /// </summary>
        protected List<EmotionVocabulary> vocabularies = null;

        /// <summary>
        /// adds a emotion vocabulary to list
        /// </summary>
        /// <param name="vocabulary">defined emotion vocabulary</param>
        public void addVocabulary(EmotionVocabulary vocabulary)
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
                emotionml.SetAttribute("category-set", this.category.getEmotionsetUri().AbsoluteUri);
            }
            if (this.dimension != null)
            {
                emotionml.SetAttribute("dimension-set", this.dimension.getEmotionsetUri().AbsoluteUri);
            }
            if (this.appraisal != null)
            {
                emotionml.SetAttribute("appraisal-set", this.appraisal.getEmotionsetUri().AbsoluteUri);
            }
            if (this.actionTendency != null)
            {
                emotionml.SetAttribute("action-tendency-set", this.actionTendency.getEmotionsetUri().AbsoluteUri);
            }

            //add vocabularies to list
            foreach (EmotionVocabulary vocabulary in this.vocabularies)
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
