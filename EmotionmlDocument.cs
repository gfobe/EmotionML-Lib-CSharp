using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    class EmotionMLDocument
    {
        /* ## attributes ## */

        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        public Uri categorySet = null;
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        public Uri dimensionSet = null;
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        public Uri appraisalSet = null;
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        public Uri actionTendencySet = null;
        /// <summary>
        /// version of EmotionML
        /// if you do not set it here, it must be set in <emotion/> tag
        /// </summary>
        protected string version = null;

        /* ## child elements ## */

        /// <summary>
        /// info child
        /// </summary>
        protected Info info = null;
        /// <summary>
        /// emotion vacabularies
        /// </summary>
        protected List<Vocabulary> vocabularies = new List<Vocabulary>();
        /// <summary>
        /// emotions
        /// </summary>
        protected List<Emotion> emotions = new List<Emotion>();


        /* ### GETTER AND SETTER ### */

        /// <summary>
        /// category set for categories
        /// </summary>
        public Uri CategorySet {
            get { return categorySet; }
            set { categorySet = value; }
        }

        /// <summary>
        /// dimension set for dimensions
        /// </summary>
        public Uri DimensionSet
        {
            get { return dimensionSet; }
            set { dimensionSet = value; }
        }

        /// <summary>
        /// appraisal set for appraisals
        /// </summary>
        public Uri AppraisalSet
        {
            get { return appraisalSet; }
            set { appraisalSet = value; }
        }

        /// <summary>
        /// action tendency set for action tendencys
        /// </summary>
        public Uri ActionTendencySet
        {
            get { return actionTendencySet; }
            set { actionTendencySet = value; }
        }

        /// <summary>
        /// version of EmotionML
        /// </summary>
        public string Version
        {
            get
            {
                if (null == version)
                {
                    return "1.0"; //OPTIMIZE: use EmotionML.VERSION
                }

                return version;
            }
            set { version = value; }
        }


        /* ### OTHER METHODS ### */

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

            if (this.categorySet != null)
            {
                emotionml.SetAttribute("category-set", this.categorySet.AbsoluteUri);
            }
            if (this.dimensionSet != null)
            {
                emotionml.SetAttribute("dimension-set", this.dimensionSet.AbsoluteUri);
            }
            if (this.appraisalSet != null)
            {
                emotionml.SetAttribute("appraisal-set", this.appraisalSet.AbsoluteUri);
            }
            if (this.actionTendencySet != null)
            {
                emotionml.SetAttribute("action-tendency-set", this.actionTendencySet.AbsoluteUri);
            }

            //add info block
            if (info != null) 
            {
                emotionml.AppendChild(info.ToDom());
            }

            //add vocabularies to list
            foreach (Vocabulary vocabulary in this.vocabularies)
            {
                emotionml.AppendChild(vocabulary.ToDom());
            }

            //add emotions to list
            foreach (Emotion emotion in this.emotions)
            {
                emotionml.AppendChild(emotion.ToDom());
            }

            emotionmlXml.AppendChild(emotionml);

            return emotionmlXml;
        }

        /// <summary>
        /// creates EmotionML-XML
        /// </summary>
        /// <returns>XML of emotions</returns>
        public string ToXml()
        {
            return this.ToDom().ToString();
        }
    }

        //TODO: wenn wir hier emotion sets haben, brauchen die nicht mehr im emotion-bereich drin sein, ebenso version
}
