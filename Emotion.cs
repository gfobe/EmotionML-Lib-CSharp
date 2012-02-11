﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace EmotionML
{
    public class Emotion
    {
        /// <summary>
        /// set of emotion categories of current emotion annotation
        /// </summary>
        public Uri categorySet = null;
        /// <summary>
        /// set of emotion dimensions of current emotion annotation
        /// </summary>
        public Uri dimensionSet = null;
        /// <summary>
        /// set of emotion appraisals of current emotion annotation
        /// </summary>
        public Uri appraisalSet = null;
        /// <summary>
        /// set of emotion action tendencies of current emotion annotation
        /// </summary>
        public Uri actionTendencySet = null;

        /// <summary>
        /// emotions categories in current emotion annotation
        /// </summary>
        public List<EmotionCategory> category = new List<EmotionCategory>();
        /// <summary>
        /// emotions dimensions in current emotion annotation
        /// </summary>
        public List<EmotionDimension> dimension = new List<EmotionDimension>();
        /// <summary>
        /// emotions appraisals in current emotion annotation
        /// </summary>
        public List<EmotionAppraisal> appraisal = new List<EmotionAppraisal>();
        /// <summary>
        /// emotions action tendencies in current emotion annotation
        /// </summary>
        public List<EmotionActionTendency> actionTendency = new List<EmotionActionTendency>();

        /// <summary>
        /// unique id of emotion annotation
        /// </summary>
        public string id = null;
        /// <summary>
        /// references of emotion annotation
        /// </summary>
        public List<string> reference = new List<string>();

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
            if (emotionNode.Attributes["category-set"] != null)
            {
                categorySet = new Uri(emotionNode.Attributes["category-set"].InnerText);
            }
            if (emotionNode.Attributes["dimension-set"] != null)
            {
                dimensionSet = new Uri(emotionNode.Attributes["dimension-set"].InnerText);
            }
            if (emotionNode.Attributes["appraisal-set"] != null)
            {
                appraisalSet = new Uri(emotionNode.Attributes["appraisal-set"].InnerText);
            }
            if (emotionNode.Attributes["action-tendency-set"] != null)
            {
                actionTendencySet = new Uri(emotionNode.Attributes["action-tendency-set"].InnerText);
            }
            
            //there must be at least one set defined
            if (categorySet == null && dimensionSet == null && appraisalSet == null && actionTendencySet == null)
            {
                throw new Exception("At least one EmotionSet must be defined.");
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

                addCategory(new EmotionCategory(categoryName, categoryValue, categoryConfidence));
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

                addDimension(new EmotionDimension(dimensionName, dimensionValue, dimensionConfidence));
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

                addAppraisal(new EmotionAppraisal(appraisalName, appraisalValue, appraisalConfidence));
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

                addActionTendency(new EmotionActionTendency(actionTendencyName, actionTendencyValue, actionTendencyConfidence));
            }

            //add references
            XmlNodeList references = emotionNode.SelectNodes("emo:reference", nsManager);
            foreach (XmlNode refs in references) {
                reference.Add(refs.ToString());
            }
        }

        /// <summary>
        /// sets a default category-set, dimension-set, appraisal-set and action-tendency-set
        /// </summary>
        public void setDefaultSets()
        {
            categorySet = new Uri(EmotionML.CATEGORY_BIG6);
            dimensionSet = new Uri(EmotionML.DIMENSION_PAD);
            appraisalSet = new Uri(EmotionML.APPRAISAL_SCHERER);
            actionTendencySet = new Uri(EmotionML.ACTIONTENDENCY_FRIJDA);
        }




        //TODO: wozu wenn public?
        public void setCategorySet(string categorySetUri)
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
        }

        /// <summary>
        /// modify the values of a category, adds it if not exists
        /// </summary>
        /// <param name="newCategory">the values to set the category to</param>
        public void addCategory(EmotionCategory newCategory)
        {
            int foundOnIndex = category.FindIndex(delegate(EmotionCategory categoryToMatch)
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
            int foundOnIndex = category.FindIndex(delegate(EmotionCategory categoryToMatch)
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
        public void addDimension(EmotionDimension newDimension)
        {
            int foundOnIndex = dimension.FindIndex(delegate(EmotionDimension dimensionToMatch)
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
            int foundOnIndex = dimension.FindIndex(delegate(EmotionDimension dimensionToMatch)
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
        public void addAppraisal(EmotionAppraisal newAppraisal)
        {
            int foundOnIndex = appraisal.FindIndex(delegate(EmotionAppraisal appraisalToMatch)
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
            int foundOnIndex = appraisal.FindIndex(delegate(EmotionAppraisal appraisalToMatch)
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
        public void addActionTendency(EmotionActionTendency newactionTendency)
        {
            int foundOnIndex = actionTendency.FindIndex(delegate(EmotionActionTendency actionTendencyToMatch)
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
            int foundOnIndex = actionTendency.FindIndex(delegate(EmotionActionTendency actionTendencyToMatch)
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
            reference.Add(newReference);
        }

        /// <summary>
        /// deletes a reference by value
        /// </summary>
        /// <param name="reference">the value of the reference</param>
        /// <returns>deletion succeded</returns>
        public bool deleteReference(string referenceName)
        {
            int foundOnIndex = reference.FindIndex(delegate(string referenceToMatch)
            {                
                return ( referenceToMatch.CompareTo(referenceName) == 0 );
            });
            if (foundOnIndex != -1)
            {
                reference.RemoveAt(foundOnIndex);
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
            mergingEmotion.category.ForEach(delegate(EmotionCategory cat)
            {
                addCategory(cat);
            });
            mergingEmotion.dimension.ForEach(delegate(EmotionDimension dim)
            {
                addDimension(dim);
            });
            mergingEmotion.appraisal.ForEach(delegate(EmotionAppraisal apr)
            {
                addAppraisal(apr);
            });
            mergingEmotion.actionTendency.ForEach(delegate(EmotionActionTendency act)
            {
                addActionTendency(act);
            });
        }


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
                emotion.SetAttribute("category-set", categorySet.AbsoluteUri);
                foreach (EmotionCategory cat in category) {
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
                emotion.SetAttribute("dimension-set", dimensionSet.AbsoluteUri);
                dimension.ForEach(delegate(EmotionDimension dim)
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
                emotion.SetAttribute("appraisal-set", appraisalSet.AbsoluteUri);
                appraisal.ForEach(delegate(EmotionAppraisal apr)
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
                emotion.SetAttribute("action-tendency-set", actionTendencySet.AbsoluteUri);
                actionTendency.ForEach(delegate(EmotionActionTendency act)
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
            reference.ForEach(delegate(string referenceValue)
            {
                XmlElement referenceElement = emotionXml.CreateElement("reference");
                referenceElement.SetAttribute("uri", referenceValue);
                emotion.AppendChild(referenceElement);
            });

            return emotionXml;
        }

        public string ToXml()
        {
            return ToDom().ToString();
        }

    }
}