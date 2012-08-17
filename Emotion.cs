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

        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        //public List<EmotionCategory> category = new List<EmotionCategory>();
        public Set<Category> category = new Set<Category>();
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        //public List<EmotionDimension> dimension = new List<EmotionDimension>();
        public Set<Dimension> dimension = new Set<Dimension>();
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        //public List<EmotionAppraisal> appraisal = new List<EmotionAppraisal>();
        public Set<Appraisal> appraisal = new Set<Appraisal>();
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        //public List<EmotionActionTendency> actionTendency = new List<EmotionActionTendency>();
        Set<ActionTendency> actionTendency = new Set<ActionTendency>();

        /// <summary>
        /// unique id of emotion annotation
        /// </summary>
        public string id = null;
        /// <summary>
        /// references of emotion annotation
        /// </summary>
        public List<string> references = new List<string>();

        /// <summary>
        /// modality throught which an emotion is produced
        /// space delimeted set of values
        /// </summary>
        public string expressedThrough = null;

        /// <summary>
        /// info block with forther other informations about the emotion
        /// </summary>
        public Info info = null;

        //### timestamps ###

        /// <summary>
        /// denote the starting time of emotion
        /// milliseconds since 1970-01-01 0:00:00 GMT (xsd:nonNegativeInteger)
        /// </summary>
        public int? start = null; //TODO:validate //TODO: get/set
        /// <summary>
        /// denote the ending time of emotion
        /// milliseconds since 1970-01-01 0:00:00 GMT (xsd:nonNegativeInteger)
        /// </summary>
        public int? end = null; //TODO:validate 
        //Yes, I know; it's bad that it isn't a xsd:dateTime, but it has to work with EMMA //TODO: prüfen und URL dran
        /// <summary>
        /// duration of the event in milliseconds (xsd:nonNegativeInteger)
        /// </summary>
        public int? duration = null;  //TODO:validate 

        // ### relative times ###
        /// <summary>
        /// indicating the URI used to anchor the relative timestamp (xsd:anyURI)
        /// </summary>
        public Uri timeRefUri = null;
        /// <summary>
        /// indicates from wich time the relative time is measured (start=default or end)
        /// </summary>
        public int? timeRefAnchorPoint = null; //TODO: default start
        /// <summary>
        /// offset in milliseconds for the start of input from the anchor point
        /// </summary>
        public int? offsetToStart = null; //TODO: default 0, Abhängig von timeRefUri und timeRefAnchorPoint

        //TODO: <trace>



        public Emotion()
        {
  //          initNamespaceHandling();
        }

        /// <summary>
        /// create an emotion annotation out of EmotionML
        /// </summary>
        /// <param name="xml">EmotionML string (emotion-part including min. one set)</param>
        public Emotion(string xml)
        {
            XmlDocument emotionml = new XmlDocument();
            emotionml.LoadXml(xml);

 //           initNamespaceHandling();
            parseEmotionML(emotionml.DocumentElement);
        }

        /// <summary>
        /// create an emotion annotation out of a XmlNode
        /// </summary>
        /// <param name="emotion">Emotion-XMLNode</param>
        public Emotion(XmlNode emotion)
        {
//            initNamespaceHandling();
            parseEmotionML(emotion);
        }


        /// <summary>
        /// parses the whole staff of Emotion in EmotionML
        /// </summary>
        /// <param name="emotionNode"></param>
        protected void parseEmotionML(XmlNode emotionNode) {
            //init namespacemanager
            XmlNamespaceManager nsManager = new XmlNamespaceManager(emotionNode.OwnerDocument.NameTable);
            nsManager.AddNamespace("emo", EmotionML.NAMESPACE);

            //add general stuff
            if (emotionNode.Attributes["id"] != null)
            {
                id = emotionNode.Attributes["id"].InnerText;
            }

            //references
            XmlNodeList referenceNodes = emotionNode.SelectNodes("emo:reference", nsManager);
            foreach (XmlNode refs in referenceNodes)
            {
                references.Add(refs.ToString());
            }

            //infoblock
            info.Content = (XmlElement)emotionNode.SelectSingleNode("./info");
            //TODO: Exception wenn nicht vorhanden oder null?
            //expressedThrough
            if (emotionNode.Attributes["expressedThrough"] != null)
            {
                expressedThrough = emotionNode.Attributes["expressedThrough"].Value;
            }

            //add emotion sets
            if (emotionNode.Attributes["category-set"] != null)
            {
                category.Uri = new Uri(emotionNode.Attributes["category-set"].InnerText);
            }
            if (emotionNode.Attributes["dimension-set"] != null)
            {
                dimension.Uri = new Uri(emotionNode.Attributes["dimension-set"].InnerText);
            }
            if (emotionNode.Attributes["appraisal-set"] != null)
            {
                appraisal.Uri = new Uri(emotionNode.Attributes["appraisal-set"].InnerText);
            }
            if (emotionNode.Attributes["action-tendency-set"] != null)
            {
                actionTendency.Uri = new Uri(emotionNode.Attributes["action-tendency-set"].InnerText);
            }
            
            //add categories
            XmlNodeList categories = emotionNode.SelectNodes("emo:category", nsManager);
            foreach (XmlNode cat in categories)
            {
                string categoryName  = cat.Attributes["name"].InnerText;
                float? categoryValue = null;
                float? categoryConfidence = null;

                if (cat.Attributes["value"] != null) 
                {
                    categoryValue = float.Parse(cat.Attributes["value"].InnerText);
                }
                if (cat.Attributes["confidence"] != null) 
                {
                    categoryConfidence = float.Parse(cat.Attributes["confidence"].InnerText);
                }

                addCategory(new Category(categoryName, categoryValue, categoryConfidence));
            }

            //add dimensions
            XmlNodeList dimensions = emotionNode.SelectNodes("emo:dimension", nsManager);
            foreach (XmlNode dim in dimensions)
            {
                string dimensionName = dim.Attributes["name"].InnerText;
                float? dimensionValue = null;
                float? dimensionConfidence = null;

                if (dim.Attributes["value"] != null)
                {
                    dimensionValue = float.Parse(dim.Attributes["value"].InnerText);
                }
                if (dim.Attributes["confidence"] != null)
                {
                    dimensionConfidence = float.Parse(dim.Attributes["confidence"].InnerText);
                }

                addDimension(new Dimension(dimensionName, dimensionValue, dimensionConfidence));
            }

            //add appraisals
            XmlNodeList appraisals = emotionNode.SelectNodes("emo:appraisal", nsManager);
            foreach (XmlNode apr in appraisals)
            {
                string appraisalName = apr.Attributes["name"].InnerText;
                float? appraisalValue = null;
                float? appraisalConfidence = null;

                if (apr.Attributes["value"] != null)
                {
                    appraisalValue = float.Parse(apr.Attributes["value"].InnerText);
                }
                if (apr.Attributes["confidence"] != null)
                {
                    appraisalConfidence = float.Parse(apr.Attributes["confidence"].InnerText);
                }

                addAppraisal(new Appraisal(appraisalName, appraisalValue, appraisalConfidence));
            }

            //add action tendencies
            XmlNodeList actionTendencies = emotionNode.SelectNodes("emo:action-tendency", nsManager);
            foreach (XmlNode act in actionTendencies)
            {
                string actionTendencyName = act.Attributes["name"].InnerText;
                float? actionTendencyValue = null;
                float? actionTendencyConfidence = null;

                if (act.Attributes["value"] != null)
                {
                    actionTendencyValue = float.Parse(act.Attributes["value"].InnerText);
                }
                if (act.Attributes["confidence"] != null)  {
                    actionTendencyConfidence = float.Parse(act.Attributes["confidence"].InnerText);
                }

                addActionTendency(new ActionTendency(actionTendencyName, actionTendencyValue, actionTendencyConfidence));
            }

            //there must be at least one set defined
            if (category.Uri == null && dimension.Uri == null && appraisal.Uri == null && actionTendency.Uri == null)
            {
                //TODO: Prüfen, ob wirklich eins angegeben werden muss oder nur, wenn Emotionen da sind
                //TODO: sagen welches Set fehlt. Wenn ich eine Category habe, nutzt ein DimensionSet nicht viel
                throw new EmotionMLException("At least one EmotionSet must be defined.");
            }

            //add time ralted stuff
            if(emotionNode.Attributes["start"] != null) {
                start = Convert.ToInt32(emotionNode.Attributes["start"].Value);
            }
            if(emotionNode.Attributes["end"] != null) {
                this.end = Convert.ToInt32(emotionNode.Attributes["end"].Value);
            }
            if(emotionNode.Attributes["duration"] != null) {
                duration = Convert.ToInt32(emotionNode.Attributes["end"].Value);
            }
            if(emotionNode.Attributes["timeRefUri"] != null) { //TODO groß/kleinschreibung
                timeRefUri = new Uri(emotionNode.Attributes["timeRefUri"].Value);
            }
            if(emotionNode.Attributes["timeRefAnchorPoint"] != null) {
                timeRefAnchorPoint = Convert.ToInt32(emotionNode.Attributes["timeRefAnchorPoint"].Value);
            }
            if(emotionNode.Attributes["offsetToStart"] != null) {
                timeRefAnchorPoint = Convert.ToInt32(emotionNode.Attributes["offsetToStart"].Value);
            }
            //TODO: Abhängigkeiten prüfen
        }

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



        //TODO: wozu wenn public?
       /* public void setCategorySet(string categorySetUri)
        {
            categorySet = new Uri(categorySetUri);
        }
        public void setCategorySet(Uri categorySetUri)
        {
            categorySet = categorySetUri;
        }

        public void setDimensionSet(string dimensionSetUri)
        {
            dimensionSet = new Uri(dimensionSetUri);
        }
        public void setDimensionSet(Uri dimensionSetUri)
        {
            dimensionSet = dimensionSetUri;
        }

        public void setAppraisalSet(string appraisalSetUri)
        {
            appraisalSet = new Uri(appraisalSetUri);
        }
        public void setAppraisalSet(Uri appraisalSetUri)
        {
            appraisalSet = appraisalSetUri;
        }

        public void setActionTendencySet(string actionTendencySetUri)
        {
            actionTendencySet = new Uri(actionTendencySetUri);
        }
        public void setActionTendencySet(Uri actionTendencySetUri)
        {
            actionTendencySet = actionTendencySetUri;
        }*/

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
                category[foundOnIndex].value = newCategory.value;
                category[foundOnIndex].confidence = newCategory.confidence;
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
                dimension[foundOnIndex].value = newDimension.value;
                dimension[foundOnIndex].confidence = newDimension.confidence;
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
                appraisal[foundOnIndex].value = newAppraisal.value;
                appraisal[foundOnIndex].confidence = newAppraisal.confidence;
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
                actionTendency[foundOnIndex].value = newactionTendency.value;
                actionTendency[foundOnIndex].confidence = newactionTendency.confidence;
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
        /// name the emotion annotation with an id
        /// </summary>
        /// <param name="newid">id for emotion annotation</param>
        public void setId(string newid)
        {
            id = newid;
        }

        /// <summary>
        /// adds a reference for emotion annotation
        /// </summary>
        /// <param name="newReference">the reference URL</param>
        public void addReference(string newReference)
        {
            //TODO: gegen Aufbau xml:ID prüfen
            references.Add(newReference);
        }

        /// <summary>
        /// deletes a reference by value
        /// </summary>
        /// <param name="reference">the value of the reference</param>
        /// <returns>deletion succeded</returns>
        public bool deleteReference(string referenceName)
        {
            int foundOnIndex = references.FindIndex(delegate(string referenceToMatch)
            {                
                return ( referenceToMatch.CompareTo(referenceName) == 0 );
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
                    if (cat.value != null)
                    {
                        catNode.SetAttribute("value", cat.value.ToString());
                    }
                    if (cat.confidence != null)
                    {
                        catNode.SetAttribute("confidence", cat.confidence.ToString());
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
                    if (dim.value != null)
                    {
                        dimNode.SetAttribute("value", dim.value.ToString());
                    }
                    if (dim.confidence != null)
                    {
                        dimNode.SetAttribute("confidence", dim.confidence.ToString());
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
                    if (apr.value != null)
                    {
                        aprNode.SetAttribute("value", apr.value.ToString());
                    }
                    if (apr.confidence != null)
                    {
                        aprNode.SetAttribute("confidence", apr.confidence.ToString());
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
                    if (act.value != null)
                    {
                        actNode.SetAttribute("value", act.value.ToString());
                    }
                    if (act.confidence != null)
                    {
                        actNode.SetAttribute("confidence", act.confidence.ToString());
                    }
                    emotion.AppendChild(actNode);
                });
            }

            //loop trought <reference>
            references.ForEach(delegate(string referenceValue)
            {
                XmlElement referenceElement = emotionXml.CreateElement("reference");
                referenceElement.SetAttribute("uri", referenceValue);
                emotion.AppendChild(referenceElement);
            });

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