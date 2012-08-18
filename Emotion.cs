using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    //FIXME: bei W3C nachfragen: Attribut time-ref-anchor-point hat default bei start, aber start selbst hat kein default und kann angegeben werden
    public class Emotion
    {
        // defined entries for attribute expressed-throught in EmotionML, but also other are possible
        const string EXPRESSED_THROUGHT_GAZE = "gaze";
        const string EXPRESSED_THROUGHT_FACE = "face";
        const string EXPRESSED_THROUGHT_HEAD = "head";
        const string EXPRESSED_THROUGHT_TORSO = "torso";
        const string EXPRESSED_THROUGHT_GESTURE = "gesture";
        const string EXPRESSED_THROUGHT_LEG = "leg";
        const string EXPRESSED_THROUGHT_VOICE = "voice";
        const string EXPRESSED_THROUGHT_TEXT = "text";
        const string EXPRESSED_THROUGHT_LOCOMOTION = "locomotion";
        const string EXPRESSED_THROUGHT_POSTURE = "posture";
        const string EXPRESSED_THROUGHT_PHYSIOLOGY = "physiology";
        const string EXPRESSED_THROUGHT_CAMERA = "camera";
        const string EXPRESSED_THROUGHT_MICROPHONE = "microphone";


        /* ## CHILD TAGS ## */

        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        protected Set<Category> category = new Set<Category>();
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        protected Set<Dimension> dimension = new Set<Dimension>();
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        protected Set<Appraisal> appraisal = new Set<Appraisal>();
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        protected Set<ActionTendency> actionTendency = new Set<ActionTendency>();

        /// <summary>
        /// references of emotion annotation
        /// </summary>
        protected List<Reference> references = new List<Reference>();
        /// <summary>
        /// info block with forther other informations about the emotion
        /// </summary>
        protected Info info = null;


        /* ## ATTRIBUTES ## */
        /// <summary>
        /// unique id of emotion annotation
        /// </summary>
        protected string id = null;
        /// <summary>
        /// version of EmotionML
        /// optional if you set it in surrounding <emotionml/> tag
        /// </summary>
        protected string version = null;
        /// <summary>
        /// modality throught which an emotion is produced
        /// space delimeted set of values
        /// </summary>
        protected string expressedThrough = null;

        /* # timestamps # */
        //It isn't a xsd:dateTime, because it has to work with EMMA and other
        //http://lists.w3.org/Archives/Public/www-multimodal/2011Sep/0001.html

        /// <summary>
        /// denote the starting time of emotion
        /// milliseconds since 1970-01-01 0:00:00 GMT (xsd:nonNegativeInteger)
        /// </summary>
        protected int? start = null;
        /// <summary>
        /// denote the ending time of emotion
        /// milliseconds since 1970-01-01 0:00:00 GMT (xsd:nonNegativeInteger)
        /// </summary>
        protected int? end = null;
        /// <summary>
        /// duration of the event in milliseconds (xsd:nonNegativeInteger)
        /// </summary>
        protected int? duration = null;

        /* # relative times # */

        /// <summary>
        /// indicating the URI used to anchor the relative timestamp (xsd:anyURI)
        /// </summary>
        protected Uri timeRefUri = null;
        /// <summary>
        /// indicates from wich time the relative time is measured (start=default or end)
        /// </summary>
        protected int? timeRefAnchorPoint = null;
        /// <summary>
        /// offset in milliseconds for the start of input from the anchor point
        /// </summary>
        protected int? offsetToStart = null;

        /* # other # */
        /// <summary>
        /// plaintext in <emotion/>
        /// </summary>
        protected string plaintext = null;


        /* ### GETTER AND SETTER ### */

        public Info Info
        {
            get { return info; }
            set { info = value; }
        }

        public Set<Category> Categories
        {
            get { return category; }
            set { category = value; }
        }

        public Set<Dimension> Dimensions
        {
            get { return dimension; }
            set { dimension = value; }
        }

        public Set<Appraisal> Appraisals
        {
            get { return appraisal; }
            set { appraisal = value; }
        }

        public Set<ActionTendency> ActionTendencies
        {
            get { return actionTendency; }
            set { actionTendency = value; }
        }

        public string Id
        {
            get { return id; }
            set
            {
                //TODO: validate xsd:ID
                id = value;
            }
        }

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

        public string ExpressedThrough {
            get { return expressedThrough; }
            set
            {
                //TODO: validate
                expressedThrough = value;
            }
        }

        public int? Start
        {
            get { return start; }
            set
            {
                if (value != null && value < 0)
                {
                    throw new EmotionMLException("only possitive values are allowed as start time");
                }
                if (end != null)
                {
                    if (value > end)
                    {
                        throw new EmotionMLException("start time must be before end time");
                    }
                }
                start = value;
            }
        }

        public int? End
        {
            get { return end; }
            set
            {
                if (value != null && value < 0)
                {
                    throw new EmotionMLException("only possitive values are allowed as end time");
                }
                if (start != null)
                {
                    if (start > value)
                    {
                        throw new EmotionMLException("end time must be after start time");
                    }
                }
                end = value;
            }
        }

        public int? Duration
        {
            get { return duration; }
            set
            {
                if (value < 0)
                {
                    throw new EmotionMLException("duration have to be positive");
                }
                duration = value;
            }
        }

        public Uri TimeRefUri
        {
            get { return timeRefUri; }
            set { timeRefUri = value; }
        }

        public int? TimeRefAnchorPoint
        {
            get {
                if (null == timeRefAnchorPoint)
                {
                    //default start
                    return start;
                }

                return timeRefAnchorPoint; 
            }
            set {
                if (start != value && end != value)
                {
                    throw new EmotionMLException("refAnchorPoint has to be value of start or end");
                }
                timeRefAnchorPoint = value;
            }
        }

        public int? OffsetToStart
        {
            get
            {
                if (null == offsetToStart)
                {
                    return 0;
                }
                return offsetToStart;
            }
            set { offsetToStart = value; }
        }

        public string Plaintext
        {
            get { return plaintext; }
            set { plaintext = value; }
        }


        /* ### PUBLIC METHODS ### */ 

        /// <summary>
        /// sets a default category-set, dimension-set, appraisal-set and action-tendency-set
        /// </summary>
        public void setDefaultSets()
        {
            category.Uri = new Uri(Category.CATEGORY_BIG6 );
            dimension.Uri = new Uri(Dimension.DIMENSION_PAD );
            appraisal.Uri = new Uri(Appraisal.APPRAISAL_SCHERER );
            actionTendency.Uri = new Uri(ActionTendency.ACTIONTENDENCY_FRIJDA );
        }

        /// <summary>
        /// compares this emotion with another for equality
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public bool equalsEmotion(Emotion emotion)
        {
            //TODO: to emotions are equal?

            return false;
        }

        /// <summary>
        /// modify the values of a category, adds it if not exists
        /// </summary>
        /// <param name="newCategory">the values to set the category to</param>
        public void addCategory(Category newCategory)
        {
            int foundOnIndex = category.FindIndex(delegate(Category categoryToMatch)
                {
                    return categoryToMatch.name == newCategory.name;
                });


            if (foundOnIndex == -1)
            {
                category.Add(newCategory);
            }
            else
            {
                category[foundOnIndex].Value = newCategory.Value;
                category[foundOnIndex].Confidence = newCategory.Confidence;
            }
        }

        /// <summary>
        /// deletes a category by name
        /// </summary>
        /// <param name="categoryName">the name of the category to delete</param>
        /// <returns>deletion succeded</returns>
        public bool deleteCategory(string categoryName) {
            int foundOnIndex = category.FindIndex(delegate(Category categoryToMatch)
            {
                return categoryToMatch.name == categoryName;
            });
            if (foundOnIndex != -1)
            {
                category.RemoveAt(foundOnIndex);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// modify the values of a dimension, adds it if not exists
        /// </summary>
        /// <param name="newDimension">the values to set the dimension to</param>
        public void addDimension(Dimension newDimension)
        {
            int foundOnIndex = dimension.FindIndex(delegate(Dimension dimensionToMatch)
            {
                return dimensionToMatch.name == newDimension.name;
            });
            if (foundOnIndex == -1)
            {
                dimension.Add(newDimension);
            }
            else
            {
                dimension[foundOnIndex].Value = newDimension.Value;
                dimension[foundOnIndex].Confidence = newDimension.Confidence;
            }
        }

        /// <summary>
        /// deletes a dimension by name
        /// </summary>
        /// <param name="dimensionName">the name of the dimension to delete</param>
        /// <returns>deletion succeded</returns>
        public bool deleteDimension(string dimensionName)
        {
            int foundOnIndex = dimension.FindIndex(delegate(Dimension dimensionToMatch)
            {
                return dimensionToMatch.name == dimensionName;
            });
            if (foundOnIndex != -1)
            {
                dimension.RemoveAt(foundOnIndex);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// modify the values of a appraisal, adds it if not exists
        /// </summary>
        /// <param name="newappraisal">the values to set the appraisal to</param>
        public void addAppraisal(Appraisal newAppraisal)
        {
            int foundOnIndex = appraisal.FindIndex(delegate(Appraisal appraisalToMatch)
            {
                return appraisalToMatch.name == newAppraisal.name;
            });
            if (foundOnIndex == -1)
            {
                appraisal.Add(newAppraisal);
            }
            else
            {
                appraisal[foundOnIndex].Value = newAppraisal.Value;
                appraisal[foundOnIndex].Confidence = newAppraisal.Confidence;
            }
        }

        /// <summary>
        /// deletes a appraisal by name
        /// </summary>
        /// <param name="appraisalName">the name of the appraisal to delete</param>
        /// <returns>deletion succeded</returns>
        public bool deleteAppraisal(string appraisalName)
        {
            int foundOnIndex = appraisal.FindIndex(delegate(Appraisal appraisalToMatch)
            {
                return appraisalToMatch.name == appraisalName;
            });
            if (foundOnIndex != -1)
            {
                appraisal.RemoveAt(foundOnIndex);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// modify the values of a actionTendency, adds it if not exists
        /// </summary>
        /// <param name="newactionTendency">the values to set the actionTendency to</param>
        public void addActionTendency(ActionTendency newactionTendency)
        {
            int foundOnIndex = actionTendency.FindIndex(delegate(ActionTendency actionTendencyToMatch)
            {
                return actionTendencyToMatch.name == newactionTendency.name;
            });
            if (foundOnIndex == -1)
            {
                actionTendency.Add(newactionTendency);
            }
            else
            {
                actionTendency[foundOnIndex].Value = newactionTendency.Value;
                actionTendency[foundOnIndex].Confidence = newactionTendency.Confidence;
            }
        }

        /// <summary>
        /// deletes a actionTendency by name
        /// </summary>
        /// <param name="actionTendencyName">the name of the actionTendency to delete</param>
        /// <returns>deletion succeded</returns>
        public bool deleteActionTendency(string actionTendencyName)
        {
            int foundOnIndex = actionTendency.FindIndex(delegate(ActionTendency actionTendencyToMatch)
            {
                return actionTendencyToMatch.name == actionTendencyName;
            });
            if (foundOnIndex != -1)
            {
                actionTendency.RemoveAt(foundOnIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// adds a reference for emotion annotation
        /// </summary>
        /// <param name="newReference">the reference URL</param>
        public void addReference(Reference newReference)
        {
            references.Add(newReference);
        }

        /// <summary>
        /// deletes a reference by value
        /// </summary>
        /// <param name="reference">the value of the reference</param>
        /// <returns>deletion succeded</returns>
        public bool deleteReference(Reference referenceName)
        {
            int foundOnIndex = references.FindIndex(delegate(Reference referenceToMatch)
            {                
                //OPTIMIZE: let reference decide if another reference is equal
                return ( referenceToMatch.Uri == referenceName.Uri );
            });
            if (foundOnIndex != -1)
            {
                references.RemoveAt(foundOnIndex);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// merges this emotion with another emotion annnotation
        /// </summary>
        /// <param name="mergingEmotion">emotion to integrate in this emotion</param>
        public void mergeWithEmotion(Emotion mergingEmotion)
        {
            mergingEmotion.category.ForEach(delegate(Category cat)
            {
                addCategory(cat);
            });
            mergingEmotion.dimension.ForEach(delegate(Dimension dim)
            {
                addDimension(dim);
            });
            mergingEmotion.appraisal.ForEach(delegate(Appraisal apr)
            {
                addAppraisal(apr);
            });
            mergingEmotion.actionTendency.ForEach(delegate(ActionTendency act)
            {
                addActionTendency(act);
            });
        }

        /// <summary>
        /// creates a DOM of Emotion
        /// </summary>
        /// <returns>DOM of emotion definition</returns>
        public XmlDocument ToDom()
        {
            XmlDocument emotionXml = new XmlDocument();

            //set up <emotion>
            XmlElement emotion = emotionXml.CreateElement("emotion");
            emotion.SetAttribute("xmlns", EmotionML.NAMESPACE);
            if(id != null) {
                emotion.SetAttribute("id", id);
            }

            //loop trought <category>
            if (category.Count > 0)
            {
                emotion.SetAttribute("category-set", category.Uri.AbsoluteUri);
                foreach (Category cat in category) {
                    XmlElement catNode = emotionXml.CreateElement("category");
                    catNode.SetAttribute("name", cat.name);
                    if (cat.Value != null)
                    {
                        catNode.SetAttribute("value", cat.Value.ToString());
                    }
                    if (cat.Confidence != null)
                    {
                        catNode.SetAttribute("confidence", cat.Confidence.ToString());
                    }
                    emotion.AppendChild(catNode);
                };
            }

            //loop trought <dimension>
            if (dimension.Count > 0)
            {
                emotion.SetAttribute("dimension-set", dimension.Uri.AbsoluteUri);
                dimension.ForEach(delegate(Dimension dim)
                {
                    XmlElement dimNode = emotionXml.CreateElement("dimension");
                    dimNode.SetAttribute("name", dim.name);
                    if (dim.Value != null)
                    {
                        dimNode.SetAttribute("value", dim.Value.ToString());
                    }
                    if (dim.Confidence != null)
                    {
                        dimNode.SetAttribute("confidence", dim.Confidence.ToString());
                    }
                    emotion.AppendChild(dimNode);
                });
            }

            //loop trought <appraisal>
            if (appraisal.Count > 0)
            {
                emotion.SetAttribute("appraisal-set", appraisal.Uri.AbsoluteUri);
                appraisal.ForEach(delegate(Appraisal apr)
                {
                    XmlElement aprNode = emotionXml.CreateElement("appraisal");
                    aprNode.SetAttribute("name", apr.name);
                    if (apr.Value != null)
                    {
                        aprNode.SetAttribute("value", apr.Value.ToString());
                    }
                    if (apr.Confidence != null)
                    {
                        aprNode.SetAttribute("confidence", apr.Confidence.ToString());
                    }
                    emotion.AppendChild(aprNode);
                });
            }

            //loop trought <action-tendency>
            if (actionTendency.Count > 0)
            {
                emotion.SetAttribute("action-tendency-set", actionTendency.Uri.AbsoluteUri);
                actionTendency.ForEach(delegate(ActionTendency act)
                {
                    XmlElement actNode = emotionXml.CreateElement("action-tendency");
                    actNode.SetAttribute("name", act.name);
                    if (act.Value != null)
                    {
                        actNode.SetAttribute("value", act.Value.ToString());
                    }
                    if (act.Confidence != null)
                    {
                        actNode.SetAttribute("confidence", act.Confidence.ToString());
                    }
                    emotion.AppendChild(actNode);
                });
            }

            //loop trought <reference>
            references.ForEach(delegate(Reference referenceValue)
            {
                emotion.AppendChild(referenceValue.ToDom());
            });

            if (plaintext != null)
            {
                emotion.AppendChild(emotionXml.CreateTextNode(plaintext));
            }

            emotionXml.AppendChild(emotion);

            return emotionXml;
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