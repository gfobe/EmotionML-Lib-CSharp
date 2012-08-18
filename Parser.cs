using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Parser
    {
        /// <summary>
        /// XML given to parse
        /// </summary>
        XmlDocument xml;

        /// <summary>
        /// resulted EmotionML document
        /// </summary>
        EmotionMLDocument emotionml = new EmotionMLDocument();

        /// <summary>
        /// handler for namespaces
        /// </summary>
        XmlNamespaceManager nsManager;

        public Parser(XmlDocument xml)
        {
            this.xml = xml;
            parse();
        }

        public Parser(string xml)
        {
            this.xml = new XmlDocument();
            this.xml.LoadXml(xml);
            parse();
        }

        public EmotionMLDocument getEmotionMLDocument()
        {
            return emotionml;
        }

        public List<Emotion> getEmotionList()
        {
            return emotionml.Emotions;
        }

        public List<Vocabulary> getVocabularies()
        {
            return emotionml.Vocabularies;
        }

        public Emotion getSingleEmotion()
        {
            return emotionml.Emotions.First<Emotion>();
        }

        /// <summary>
        /// parses the whole staff of EmotionML
        /// </summary>
        protected void parse()
        {
            init();
            parseGeneral();
            parseVocabularies();
            parseEmotions();
            validate();
        }

        protected void init()
        {
            //init namespacemanager
            nsManager = new XmlNamespaceManager(xml.OwnerDocument.NameTable);
            nsManager.AddNamespace("emo", EmotionML.NAMESPACE);
        }

        protected void validate()
        {
            //sets ids versions in <emotion> and <emotionml>
            // -> in emotionml rein
            //dimension MUST have a value or a trace, the other MAY
            //sets in emotion nur optional, wenn emotionml drumerherum -> Emotion::validateSingle()  Emotion::validate()

            //TODO: Prüfen, ob wirklich eins angegeben werden muss oder nur, wenn Emotionen da sind
            //TODO: sagen welches Set fehlt. Wenn ich eine Category habe, nutzt ein DimensionSet nicht viel
            //throw new EmotionMLException("At least one EmotionSet must be defined.");
        }


        protected void parseGeneral()
        {
            //category sets
        }

        protected void parseVocabularies()
        {
            XmlNodeList vocabularyNodes = xml.SelectNodes("emo:vocabulary", nsManager);
            foreach (XmlNode node in vocabularyNodes)
            {
                emotionml.addVocabulary(parseVocabulary(node));
            }
        }

        /// <summary>
        /// parses <vocabulary/> area to Vocabulary
        /// </summary>
        /// <param name="vocabularyNode">XML node of <vocabulary/></param>
        /// <returns>vocabulary object</returns>
        protected Vocabulary parseVocabulary(XmlNode vocabularyNode)
        {
            return new Vocabulary("TODO","TODO");
        }

        protected void parseEmotions()
        {
            XmlNodeList emotionNodes = xml.SelectNodes("emo:emotion", nsManager);
            foreach (XmlNode node in emotionNodes)
            {
                emotionml.addEmotion(parseEmotion(node));
            }
        }

        /// <summary>
        /// parses <emotion/> area to Emotion
        /// </summary>
        /// <param name="emotionNode">XML node of <emotion/></param>
        /// <returns>emotion object</returns>
        protected Emotion parseEmotion(XmlNode emotionNode)
        {
            Emotion emotion = new Emotion();

            /* ## attributes ## */

            //id
            if (emotionNode.Attributes["id"] != null)
            {
                emotion.Id = emotionNode.Attributes["id"].Value;
            }

            //version
            if (emotionNode.Attributes["version"] != null)
            {
                emotion.Version = emotionNode.Attributes["version"].Value;
            }

            //expressedThrough
            if (emotionNode.Attributes["expressedThrough"] != null)
            {
                emotion.ExpressedThrough = emotionNode.Attributes["expressedThrough"].Value;
            }

            //add time ralted stuff
            if (emotionNode.Attributes["start"] != null)
            {
                emotion.Start = Convert.ToInt32(emotionNode.Attributes["start"].Value);
            }
            if (emotionNode.Attributes["end"] != null)
            {
                emotion.End = Convert.ToInt32(emotionNode.Attributes["end"].Value);
            }
            if (emotionNode.Attributes["duration"] != null)
            {
                emotion.Duration = Convert.ToInt32(emotionNode.Attributes["end"].Value);
            }
            if (emotionNode.Attributes["time-ref-uri"] != null)
            {
                emotion.TimeRefUri = new Uri(emotionNode.Attributes["time-ref-uri"].Value);
            }
            if (emotionNode.Attributes["time-ref-anchor-point"] != null)
            {
                emotion.TimeRefAnchorPoint = Convert.ToInt32(emotionNode.Attributes["time-ref-anchor-point"].Value);
            }
            if (emotionNode.Attributes["offset-to-start"] != null)
            {
                emotion.OffsetToStart = Convert.ToInt32(emotionNode.Attributes["offset-to-start"].Value);
            }


            /* ## child tags ## */

            //references
            XmlNodeList referenceNodes = emotionNode.SelectNodes("emo:reference", nsManager);
            foreach (XmlNode refs in referenceNodes)
            {
                emotion.addReference(parseReference(refs));
            }

            //infoblock
            XmlNode infoblock = emotionNode.SelectSingleNode("emo:info", nsManager);
            if(infoblock != null) {
                Info infoArea = new Info();
                infoArea.Content = infoblock.ChildNodes;
                if(infoblock.Attributes["id"] != null) {
                    infoArea.Id = infoblock.Attributes["id"].Value;
                }
                emotion.Info = infoArea;
            }
            
            //handle emotion sets
            Uri categorySet = null;
            Uri dimensionSet = null;
            Uri appraisalSet = null;
            Uri actionTendencySet = null;

            //search emotion sets in attributes of <emotion>, otherwise use this of <emotionml> (can also be null)
            if (emotionNode.Attributes["category-set"] != null)
            {
                categorySet = new Uri(emotionNode.Attributes["category-set"].InnerText);
            }
            else
            {
                categorySet = emotionml.CategorySet;
            }
            if (emotionNode.Attributes["dimension-set"] != null)
            {
                dimensionSet = new Uri(emotionNode.Attributes["dimension-set"].InnerText);
            }
            else
            {
                dimensionSet = emotionml.DimensionSet;
            }
            if (emotionNode.Attributes["appraisal-set"] != null)
            {
                appraisalSet = new Uri(emotionNode.Attributes["appraisal-set"].InnerText);
            }
            else
            {
                appraisalSet = emotionml.AppraisalSet;
            }
            if (emotionNode.Attributes["action-tendency-set"] != null)
            {
                actionTendencySet = new Uri(emotionNode.Attributes["action-tendency-set"].InnerText);
            }
            else
            {
                actionTendencySet = emotionml.ActionTendencySet;
            }

            //add categories
            XmlNodeList categoryTags = emotionNode.SelectNodes("emo:category", nsManager);
            if (categoryTags.Count > 0)
            {
                Set<Category> categories = new Set<Category>(categorySet);
           
                foreach (XmlNode cat in categoryTags)
                {
                    string categoryName = cat.Attributes["name"].InnerText;
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

                    categories.Add(new Category(categoryName, categoryValue, categoryConfidence));
                }

                emotion.Categories = categories;
            }

            //add dimensions
            XmlNodeList dimensionTags = emotionNode.SelectNodes("emo:dimension", nsManager);
            if (dimensionTags.Count > 0)
            {
                Set<Dimension> dimensions = new Set<Dimension>(dimensionSet);
            
                foreach (XmlNode dim in dimensionTags)
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

                    dimensions.Add(new Dimension(dimensionName, dimensionValue, dimensionConfidence));
                }

                emotion.Dimensions = dimensions;
            }

            //add appraisals
            XmlNodeList appraisalTags = emotionNode.SelectNodes("emo:appraisal", nsManager);
            if (appraisalTags.Count > 0)
            {
                Set<Appraisal> appraisals = new Set<Appraisal>(appraisalSet);

                foreach (XmlNode apr in appraisalTags)
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

                    appraisals.Add(new Appraisal(appraisalName, appraisalValue, appraisalConfidence));
                }

                emotion.Appraisals = appraisals;
            }

            //add action tendencies
            XmlNodeList actionTendencyTags = emotionNode.SelectNodes("emo:action-tendency", nsManager);
            if (actionTendencyTags.Count > 0)
            {
                Set<ActionTendency> actionTendencies = new Set<ActionTendency>(actionTendencySet);

                foreach (XmlNode act in actionTendencyTags)
                {
                    string actionTendencyName = act.Attributes["name"].InnerText;
                    float? actionTendencyValue = null;
                    float? actionTendencyConfidence = null;

                    if (act.Attributes["value"] != null)
                    {
                        actionTendencyValue = float.Parse(act.Attributes["value"].InnerText);
                    }
                    if (act.Attributes["confidence"] != null)
                    {
                        actionTendencyConfidence = float.Parse(act.Attributes["confidence"].InnerText);
                    }

                    actionTendencies.Add(new ActionTendency(actionTendencyName, actionTendencyValue, actionTendencyConfidence));
                }

                emotion.ActionTendencies = actionTendencies;
            }

            return emotion;
        }

        protected Reference parseReference(XmlNode referenceNode)
        {
            return new Reference(new Uri("TODO"));
        }
    }
}
