// -- FreeBSD License ---------------------------------------------------------
// Copyright (c) 2012, Gerhard Fobe
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
// 2. Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER 
// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
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
        //in examples
        const string EXPRESSED_THROUGHT_CAMERA = "camera";
        const string EXPRESSED_THROUGHT_MICROPHONE = "microphone";
        const string EXPRESSED_THROUGHT_SKIN_COLOR = "facial-skin-color";


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
        protected long? start = null;
        /// <summary>
        /// denote the ending time of emotion
        /// milliseconds since 1970-01-01 0:00:00 GMT (xsd:nonNegativeInteger)
        /// </summary>
        protected long? end = null;
        /// <summary>
        /// duration of the event in milliseconds (xsd:nonNegativeInteger)
        /// </summary>
        protected long? duration = null;

        /* # relative times # */

        /// <summary>
        /// indicating the URI used to anchor the relative timestamp (xsd:anyURI)
        /// </summary>
        protected Uri timeRefUri = null;
        /// <summary>
        /// indicates from wich time the relative time is measured (start=default or end)
        /// </summary>
        protected long? timeRefAnchorPoint = null;
        /// <summary>
        /// offset in milliseconds for the start of input from the anchor point
        /// </summary>
        protected long? offsetToStart = null;

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
                if (Helper.isXsdId(value))
                {
                    id = value;
                }
                else
                {
                    throw new EmotionMLException("Id have to be a xsd:ID");
                }
            }
        }

        public string Version
        {
            get
            {
                if (null == version)
                {
                    return EmotionML.VERSION;
                }

                return version;
            }
            set { version = value; }
        }

        public List<Reference> References
        {
            get { return references; }
        }


        public string ExpressedThrough {
            get { return expressedThrough; }
            set
            {
                if (Helper.isNmtokens(value))
                {
                    expressedThrough = value;
                }
                else
                {
                    throw new EmotionMLException("ExpressedThrought has to be a xsd:nmtoken");
                }
            }
        }

        public long? Start
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

        public long? End
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

        public long? Duration
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

        public long? TimeRefAnchorPoint
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

        public long? OffsetToStart
        {
            get
            {
                if (null == offsetToStart)
                {
                    return 0; //default
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
        /// <param name="obj">object to compare with</param>
        /// <returns>objects are equal</returns>
        public override bool Equals(object obj)
        {
            string[] ignore = new string[] { };
            return Equals(obj, ignore);
        }

        /// <summary>
        /// compares this emotion with another for equality
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <param name="ignore">ignorations (supported: info, id)</param>
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

            Emotion control = (Emotion)obj;
            if (!ignore.Contains<string>("info"))
            {
                if (this.info == null)
                {
                    if (control.info != null)
                    {
                        return false;
                    }
                }
                else if (!this.info.Equals(control.Info))
                {
                    return false;
                }
            }
            if (!ignore.Contains<string>("id"))
            {
                if (this.id == control.Id)
                {
                    return false;
                }
            }

            if (this.Version == control.Version
            && this.Start == control.Start
            && this.End == control.End
            && this.Duration == control.Duration
            && this.TimeRefAnchorPoint == control.TimeRefAnchorPoint
            && this.OffsetToStart == control.OffsetToStart
            && this.ExpressedThrough == control.ExpressedThrough)
            {
                if (this.TimeRefUri != null) 
                {
                    if (control.TimeRefUri != null) 
                    {
                        if (this.TimeRefUri.AbsoluteUri != control.TimeRefUri.AbsoluteUri)
                        {
                            return false;
                        }
                    }
                }

                //control references
                if (this.references.Count != control.References.Count)
                {
                    return false;
                }
                foreach (Reference thisReference in this.references)
                {
                    bool continueIteration = false;
                    foreach (Reference controlReference in control.References)
                    {
                        if (thisReference.Equals(controlReference))
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

                    return false; //some reference not found
                }

                //control sets
                if (this.Categories.Uri != null) {
                    if(control.Categories.Uri != null) {
                        if(this.Categories.Uri.AbsoluteUri != control.Categories.Uri.AbsoluteUri) {
                            return false;
                        }
                    }
                }
                if (this.Dimensions.Uri != null)
                {
                    if (control.Dimensions.Uri != null)
                    {
                        if (this.Dimensions.Uri.AbsoluteUri != control.Dimensions.Uri.AbsoluteUri)
                        {
                            return false;
                        }
                    }
                }
                if (this.Appraisals.Uri != null)
                {
                    if (control.Appraisals.Uri != null)
                    {
                        if (this.Appraisals.Uri.AbsoluteUri != control.Appraisals.Uri.AbsoluteUri)
                        {
                            return false;
                        }
                    }
                }
                if (this.ActionTendencies.Uri != null)
                {
                    if (control.ActionTendencies.Uri != null)
                    {
                        if (this.ActionTendencies.Uri.AbsoluteUri != control.ActionTendencies.Uri.AbsoluteUri)
                        {
                            return false;
                        }
                    }
                }

                //control categories
                if (category.Count != control.Categories.Count)
                {
                    return false;
                }
                foreach (Category thisItem in this.category)
                {
                    bool continueIteration = false;
                    foreach (Category controlItem in control.Categories)
                    {
                        if(thisItem.Name == controlItem.Name
                        && thisItem.Value == controlItem.Value
                        && thisItem.Confidence == controlItem.Confidence
                        && thisItem.Trace == controlItem.Trace) 
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

                    return false; //some category not found
                }

                //control dimensions
                if (dimension.Count != control.Dimensions.Count)
                {
                    return false;
                }
                foreach (Dimension thisItem in this.dimension)
                {
                    bool continueIteration = false;
                    foreach (Dimension controlItem in control.Dimensions)
                    {
                        if (thisItem.Name == controlItem.Name
                        && thisItem.Value == controlItem.Value
                        && thisItem.Confidence == controlItem.Confidence
                        && thisItem.Trace == controlItem.Trace)
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

                    return false; //some dimension not found
                }

                //control appraisals
                if (appraisal.Count != control.Appraisals.Count)
                {
                    return false;
                }
                foreach (Appraisal thisItem in this.appraisal)
                {
                    bool continueIteration = false;
                    foreach (Appraisal controlItem in control.Appraisals)
                    {
                        if (thisItem.Name == controlItem.Name
                        && thisItem.Value == controlItem.Value
                        && thisItem.Confidence == controlItem.Confidence
                        && thisItem.Trace == controlItem.Trace)
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

                    return false; //some appraisal not found
                }

                //control action tendencies
                if (actionTendency.Count != control.ActionTendencies.Count)
                {
                    return false;
                }
                foreach (ActionTendency thisItem in this.actionTendency)
                {
                    bool continueIteration = false;
                    foreach (ActionTendency controlItem in control.ActionTendencies)
                    {
                        if (thisItem.Name == controlItem.Name
                        && thisItem.Value == controlItem.Value
                        && thisItem.Confidence == controlItem.Confidence
                        && thisItem.Trace == controlItem.Trace)
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

                    return false; //some action tendency not found
                }


                return true; //all important is equally
            }

            return false; //something is different
        }

        /// <summary>
        /// modify the values of a category, adds it if not exists
        /// </summary>
        /// <param name="newCategory">the values to set the category to</param>
        public void addCategory(Category newCategory)
        {
            int foundOnIndex = category.FindIndex(delegate(Category categoryToMatch)
            {
                return categoryToMatch.Name== newCategory.Name;
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
                return categoryToMatch.Name == categoryName;
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
                return dimensionToMatch.Name == newDimension.Name;
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
                return dimensionToMatch.Name == dimensionName;
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
                return appraisalToMatch.Name == newAppraisal.Name;
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
                return appraisalToMatch.Name == appraisalName;
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
                return actionTendencyToMatch.Name == newactionTendency.Name;
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
                return actionTendencyToMatch.Name == actionTendencyName;
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
                return ( referenceToMatch.Equals(referenceName) );
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
            //FIXME: merge sets also -> proplem: different sets

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
            //OPTIMIZE: refactore Emotion
            if (category == null && dimension == null && appraisal == null && actionTendency == null)
            {
                throw new EmotionMLException("At least one category, dimension, appraisal or action tendency needed.");
            }

            XmlDocument emotionXml = new XmlDocument();

            //set up <emotion>
            XmlElement emotion = emotionXml.CreateElement("emotion");
            emotion.SetAttribute("xmlns", EmotionML.NAMESPACE);
            if(id != null) {
                emotion.SetAttribute("id", id);
            }
            if (version != null)
            {
                emotion.SetAttribute("version", version);
            }
            else
            {
                emotion.SetAttribute("version", EmotionML.VERSION);
            }
            if (expressedThrough != null)
            {
                emotion.SetAttribute("expressed-through", expressedThrough);
            }
            if (start != null)
            {
                emotion.SetAttribute("start", start.ToString());
            }
            if (end != null)
            {
                emotion.SetAttribute("end", end.ToString());
            }
            if (duration != null)
            {
                emotion.SetAttribute("duration", duration.ToString());
            }
            if (timeRefUri != null)
            {
                emotion.SetAttribute("time-ref-uri", timeRefUri.ToString());
            }
            if (timeRefAnchorPoint != null)
            {
                emotion.SetAttribute("time-ref-anchor-point", timeRefAnchorPoint.ToString());
            }
            if (offsetToStart != null)
            {
                emotion.SetAttribute("offset-to-start", offsetToStart.ToString());
            }

            //loop trought <category>
            if (category.Count > 0)
            {
                emotion.SetAttribute("category-set", category.Uri.AbsoluteUri);
                foreach (Category cat in category) {
                    XmlElement catNode = emotionXml.CreateElement("category");
                    catNode.SetAttribute("name", cat.Name);
                    if (cat.Value != null)
                    {
                        catNode.SetAttribute("value", Helper.float2string(cat.Value));
                    }
                    else if (cat.Trace != null)
                    {
                        XmlNode importedNode = emotionXml.ImportNode(cat.Trace.ToDom().FirstChild, true);
                        catNode.AppendChild(importedNode);
                    }
                    if (cat.Confidence != null)
                    {
                        catNode.SetAttribute("confidence", Helper.float2string(cat.Confidence));
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
                    dimNode.SetAttribute("name", dim.Name);
                    if (dim.Value != null)
                    {
                        dimNode.SetAttribute("value", Helper.float2string(dim.Value));
                    }
                    else if (dim.Trace != null)
                    {
                        XmlNode importedNode = emotionXml.ImportNode(dim.Trace.ToDom().FirstChild, true);
                        dimNode.AppendChild(importedNode);
                    }
                    if (dim.Confidence != null)
                    {
                        dimNode.SetAttribute("confidence", Helper.float2string(dim.Confidence));
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
                    aprNode.SetAttribute("name", apr.Name);
                    if (apr.Value != null)
                    {
                        aprNode.SetAttribute("value", Helper.float2string(apr.Value));
                    }
                    else if (apr.Trace != null)
                    {
                        XmlNode importedNode = emotionXml.ImportNode(apr.Trace.ToDom().FirstChild, true);
                        aprNode.AppendChild(importedNode);
                    }
                    if (apr.Confidence != null)
                    {
                        aprNode.SetAttribute("confidence", Helper.float2string(apr.Confidence));
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
                    actNode.SetAttribute("name", act.Name);
                    if (act.Value != null)
                    {
                        actNode.SetAttribute("value", Helper.float2string(act.Value));
                    } 
                    else if (act.Trace != null) 
                    {
                        XmlNode importedNode = emotionXml.ImportNode(act.Trace.ToDom().FirstChild, true);
                        actNode.AppendChild(importedNode);
                    }

                    if (act.Confidence != null)
                    {
                        actNode.SetAttribute("confidence", Helper.float2string(act.Confidence));
                    }
                    emotion.AppendChild(actNode);
                });
            }

            //loop trought <reference>
            references.ForEach(delegate(Reference referenceValue)
            {
                XmlNode importedNode = emotionXml.ImportNode(referenceValue.ToDom().FirstChild, true);
                emotion.AppendChild(importedNode);
            });

            //add <info>
            if (info != null)
            {
                XmlNode importedNode = emotionXml.ImportNode(info.ToDom().FirstChild, true);
                emotion.AppendChild(importedNode);
            }

            //add plaintext
            if (plaintext != null)
            {
                emotion.AppendChild(emotionXml.CreateTextNode(plaintext));
            }

            emotionXml.AppendChild(emotion);

            //TODO: validate against scheme (as static method)

            return emotionXml;
        }

        /// <summary>
        /// generates the XML EmotionML
        /// </summary>
        /// <returns>XML representation of emotion</returns>
        public string ToXml()
        {
            return ToDom().OuterXml;
        }

    }
}