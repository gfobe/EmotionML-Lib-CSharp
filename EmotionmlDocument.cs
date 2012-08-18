using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class EmotionMLDocument
    {
        /* ## attributes ## */

        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        protected Uri categorySet = null;
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        protected Uri dimensionSet = null;
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        protected Uri appraisalSet = null;
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        protected Uri actionTendencySet = null;
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

        /* ## other ## */
        /// <summary>
        /// plaintext in <emotionml/>
        /// </summary>
        protected string plaintext = null;


        /* ### GETTER AND SETTER ### */

        public Info Info
        {
            get { return info; }
            set { info = value; }
        }

        public List<Vocabulary> Vocabularies
        {
            get { return vocabularies; }
            set { vocabularies = value; }
        }

        public List<Emotion> Emotions
        {
            get { return emotions; }
            set { emotions = value; }
        }

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

        public string Plaintext
        {
            get { return plaintext; }
            set { plaintext = value; }
        }


        /* ### OTHER METHODS ### */

        /// <summary>
        /// compares this emotionml document with another for equality
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>objects are equal</returns>
        public override bool Equals(object obj)
        {
            string[] ignore = new string[] { };
            return Equals(obj, ignore);
        }

        /// <summary>
        /// compares this emotionml document with another for equality
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <param name="ignore">ignorations (supported: info, plaintext)</param>
        /// <returns>objects are equal</returns>
        public bool Equals(object obj, string[] ignore)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false; //wrong type
            }
            if (base.Equals(obj))
            {
                return true; //same instance
            }

            EmotionMLDocument control = (EmotionMLDocument)obj;
            if (!ignore.Contains<string>("info"))
            {
                if (!this.info.Equals(control.Info))
                {
                    return false;
                }
            }
            if (!ignore.Contains<string>("plaintext"))
            {
                if (this.plaintext != control.Plaintext)
                {
                    return false;
                }
            }
            if (this.categorySet.AbsoluteUri == control.CategorySet.AbsoluteUri
            && this.dimensionSet.AbsoluteUri == control.DimensionSet.AbsoluteUri
            && this.appraisalSet.AbsoluteUri == control.AppraisalSet.AbsoluteUri
            && this.actionTendencySet.AbsoluteUri == control.ActionTendencySet.AbsoluteUri
            && this.version == control.Version)
            {
                //iterate through vocabularies
                List<Vocabulary> controlItems = control.Vocabularies;
                if (this.vocabularies.Count != controlItems.Count)
                {
                    return false; //not same numer of items
                }

                foreach (Vocabulary testItem in this.vocabularies)
                {
                    bool continueIteration = false;
                    foreach (Vocabulary testControlItem in controlItems)
                    {
                        if (testItem.Equals(testControlItem))
                        {
                            continueIteration = true;
                            break;
                            //why C# hasn't a continue 2?
                        }
                    }
                    if (continueIteration)
                    {
                        continueIteration = false;
                        continue;
                    }

                    return false; //current testItem not found in items of control object
                }

                //iterate through emotions
                List<Emotion> controlEmotions = control.Emotions;
                if (this.emotions.Count != controlEmotions.Count)
                {
                    return false; //not same numer of items
                }

                foreach (Emotion testItem in this.emotions)
                {
                    bool continueIteration = false;
                    foreach (Emotion testControlItem in controlEmotions)
                    {
                        if (testItem.Equals(testControlItem))
                        {
                            continueIteration = true;
                            break;
                            //why C# hasn't a continue 2?
                        }
                    }
                    if (continueIteration)
                    {
                        continueIteration = false;
                        continue;
                    }

                    return false; //current testItem not found in items of control object
                }


                return true; //all items found
            }
            else
            {
                return false; //something simple is wrong
            }
        }

        /// <summary>
        /// adds an vocabulary to document
        /// </summary>
        /// <param name="vocabulary">defined emotion vocabulary</param>
        public void addVocabulary(Vocabulary vocabulary)
        {
            this.vocabularies.Add(vocabulary);
        }

        /// <summary>
        /// adds an emotion to document
        /// </summary>
        /// <param name="emotion">the emotion</param>
        public void addEmotion(Emotion emotion)
        {
            this.emotions.Add(emotion);
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

            if (plaintext != null)
            {
                emotionmlXml.AppendChild(emotionmlXml.CreateTextNode(plaintext));
            }

            //TODO: validate against scheme (as static method)

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
