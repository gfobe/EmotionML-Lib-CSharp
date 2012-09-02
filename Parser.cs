using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Schema;

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

        public List<Emotion> getEmotions()
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
        /// <param name="ignoreSchema">do not validate input against EmotionML schema</param>
        public void parse(bool ignoreSchema = true)
        {
            init();
            if (!ignoreSchema)
            {
                isValidAgainstSchema();
            }

            //TODO: named entities auflösen: http://msdn.microsoft.com/en-us/library/system.xml.xmlnodereader.resolveentity(v=vs.71).aspx

            //filter rootnode
            XmlNode root = null;
            XmlNode documentNode = xml.SelectSingleNode("/");
            XmlNodeList documentNodes = documentNode.ChildNodes;
            foreach(XmlNode testNode in documentNodes) {
                if(testNode.GetType() == typeof(XmlElement)) {
                    root = testNode;
                    break;
                }
            }
            if (root == null)
            {
                throw new EmotionMLException("Can not find the root element.");
            }

            //start work
            if (root.Name == "emotionml")
            {
                parseEmotionMLDocument(root);

                //loop through vocabularies
                XmlNodeList vocabularyNodes = root.SelectNodes("emo:vocabulary", nsManager);
                foreach (XmlNode node in vocabularyNodes)
                {
                    emotionml.addVocabulary(parseVocabulary(node));
                }

                //loop through emotions
                XmlNodeList emotionNodes = root.SelectNodes("emo:emotion", nsManager);
                foreach (XmlNode node in emotionNodes)
                {
                    emotionml.addEmotion(parseEmotion(node));
                }
            }
            else if (root.Name == "vocabulary")
            {
                emotionml.addVocabulary(parseVocabulary(root));
            }
            else if (root.Name == "emotion")
            {
                emotionml.addEmotion(parseEmotion(root));
            }
            else
            {
                throw new EmotionMLException("Can only handle root elements <emotionml/> <vocabulary/> and <emotion/>");
            }

            validate();
        }

        /// <summary>
        /// initalisation of parser
        /// </summary>
        protected void init()
        {
            //init namespacemanager
            nsManager = new XmlNamespaceManager(xml.NameTable);
            nsManager.AddNamespace("emo", EmotionML.NAMESPACE);
        }

        protected bool isValidAgainstSchema()
        {
            string schemaStringGeneral = Helper.loadInternalResource("emotionml.xsd");
            string schemaStringFragments = Helper.loadInternalResource("emotionml-fragments.xsd");
            string xmlString = emotionml.toXmlDocument();

            //load xml document
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);

            //add schemata
            XmlSchema schema = XmlSchema.Read(new StringReader(schemaStringGeneral), null);
            xml.Schemas.Add(schema);
            schema = XmlSchema.Read(new StringReader(schemaStringFragments), null);
            xml.Schemas.Add(schema);

            //TODO: schauen, ob als Input ein dokument oder ein fragment kommt

            try
            {
                xml.Validate(null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool validate()
        {
            //TODO

            //sets ids versions in <emotion> and <emotionml>
            // -> in emotionml rein
            //dimension MUST have a value or a trace, the other MAY
            //sets in emotion nur optional, wenn emotionml drumerherum -> Emotion::validateSingle()  Emotion::validate()

            //TODO: Prüfen, ob wirklich eins angegeben werden muss oder nur, wenn Emotionen da sind
            //TODO: sagen welches Set fehlt. Wenn ich eine Category habe, nutzt ein DimensionSet nicht viel
            //throw new EmotionMLException("At least one EmotionSet must be defined.");

            return false;
        }

        /// <summary>
        /// parse general things of <emotionml/>
        /// </summary>
        protected void parseEmotionMLDocument(XmlNode emotionmlNode)
        {
            emotionml.Version = emotionmlNode.Attributes["version"].Value;
            if (emotionmlNode.Attributes["category-set"] != null)
            {
                emotionml.CategorySet = new Uri(emotionmlNode.Attributes["category-set"].Value);
            }
            if (emotionmlNode.Attributes["dimension-set"] != null)
            {
                emotionml.DimensionSet = new Uri(emotionmlNode.Attributes["dimension-set"].Value);
            }
            if (emotionmlNode.Attributes["appraisal-set"] != null)
            {
                emotionml.AppraisalSet = new Uri(emotionmlNode.Attributes["appraisal-set"].Value);
            }
            if (emotionmlNode.Attributes["action-tendency-set"] != null)
            {
                emotionml.ActionTendencySet = new Uri(emotionmlNode.Attributes["action-tendency-set"].Value);
            }

            XmlNodeList infoNodes = emotionmlNode.SelectNodes("./emo:info", nsManager);
            if (infoNodes.Count > 1)
            {
                throw new EmotionMLException("Only maximum one instance of <info/> is allowed. " + infoNodes.Count + " given.");
            }
            else if (infoNodes.Count == 1)
            {
                emotionml.Info = parseInfo(infoNodes.Item(0));
            }

            XmlNode textnode = emotionmlNode.SelectSingleNode("text()");
            if (textnode != null)
            {
                emotionml.Plaintext = textnode.InnerText;
            }
        }

        /// <summary>
        /// parses <vocabulary/> area to Vocabulary
        /// </summary>
        /// <param name="vocabularyNode">XML node of <vocabulary/></param>
        /// <returns>vocabulary object</returns>
        protected Vocabulary parseVocabulary(XmlNode vocabularyNode)
        {
            Vocabulary vocabulary = null;
            string id = vocabularyNode.Attributes["id"].Value;
            string type = vocabularyNode.Attributes["type"].Value;

            XmlNodeList items = vocabularyNode.SelectNodes("./emo:item", nsManager);
            if (items.Count == 0)
            {
                throw new EmotionMLException("Each vocabulary must have at least one item.");
            }

            //parse items 
            foreach (XmlNode itemNode in items)
            {
                string name = itemNode.Attributes["name"].Value;
                Item newItem = new Item(name);

                //parse item
                XmlNodeList infoNodes = itemNode.SelectNodes("./emo:info", nsManager);
                if (infoNodes.Count > 1)
                {
                    throw new EmotionMLException("Only maximum one instance of <info/> is allowed. " + infoNodes.Count + " given.");
                }
                else if (infoNodes.Count == 1)
                {
                    newItem.Info = parseInfo(infoNodes.Item(0));
                }

                //add item to vocabulary
                if (vocabulary == null)
                {
                    vocabulary = new Vocabulary(type, id, newItem);
                }
                else
                {
                    vocabulary.addItem(newItem);
                }

            }

            //search for info element
            XmlNodeList infos = vocabularyNode.SelectNodes("./emo:info", nsManager);
            if (infos.Count > 1)
            {
                    throw new EmotionMLException("Only maximum one instance of <info/> is allowed. " + infos.Count + " given.");
            }
            else if (infos.Count == 1)
            {
                vocabulary.Info = parseInfo(infos.Item(0));
            }

            return vocabulary;
        }

        /// <summary>
        /// parses <emotion/> area to Emotion
        /// </summary>
        /// <param name="emotionNode">XML node of <emotion/></param>
        /// <returns>Emotion object</returns>
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
            if (emotionNode.Attributes["expressed-through"] != null)
            {
                emotion.ExpressedThrough = emotionNode.Attributes["expressed-through"].Value;
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
            XmlNodeList infoblocks = emotionNode.SelectNodes("emo:info", nsManager);
            if (infoblocks.Count > 1)
            {
                throw new EmotionMLException("Only maximum one instance of <info/> is allowed. " + infoblocks.Count + " given.");
            }
            else if (infoblocks.Count == 1)
            {
                Info infoArea = parseInfo(infoblocks.Item(0));
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

            //search for plaintext
            XmlNode textnode = emotionNode.SelectSingleNode("text()");
            if (textnode != null)
            {
                emotion.Plaintext = textnode.InnerText;
            }

            return emotion;
        }

        /// <summary>
        /// parses <reference/> area to Reference
        /// </summary>
        /// <param name="referenceNode">XML node of <reference/></param>
        /// <returns>Reference object</returns>
        protected Reference parseReference(XmlNode referenceNode)
        {
            Uri uri = new Uri(referenceNode.Attributes["uri"].Value);
            Reference reference = new Reference(uri);

            if (referenceNode.Attributes["role"] != null)
            {
                reference.Role = referenceNode.Attributes["role"].Value;
            }
            if (referenceNode.Attributes["media-type"] != null)
            {
                reference.MediaType = referenceNode.Attributes["media-type"].Value;
            }

            return reference;
        }

        /// <summary>
        /// parse <trace/> element
        /// </summary>
        /// <param name="traceNode">XMLNode oder <trace/></param>
        /// <returns>Trace object</returns>
        protected Trace parseTrace(XmlNode traceNode)
        {
            string samples = traceNode.Attributes["samples"].Value;
            float frequency = float.Parse(traceNode.Attributes["freq"].Value);
            return new Trace(frequency, samples);
        }

        /// <summary>
        /// parse <item/> to Item
        /// </summary>
        /// <param name="itemNode">XML Node of <item/></param>
        /// <returns>Item object</returns>
        protected Item parseItem(XmlNode itemNode)
        {
            string name = itemNode.Attributes["name"].Value;
            Item item = new Item(name);

            //search for info element
            XmlNodeList infos = itemNode.SelectNodes("./emo:info", nsManager);
            if (infos.Count > 1)
            {
                throw new EmotionMLException("Only maximum one instance of <info/> is allowed. " + infos.Count + " given.");
            }
            else if (infos.Count == 1)
            {
                item.Info = parseInfo(infos.Item(0));
            }

            return item;
        }

        /// <summary>
        /// parses the <info/> section
        /// </summary>
        /// <param name="infoNode">XML node of <info/></param>
        /// <returns>Info object</returns>
        protected Info parseInfo(XmlNode infoNode)
        {
            Info infoArea = new Info();
            infoArea.Content = infoNode.ChildNodes;
            if (infoNode.Attributes["id"] != null)
            {
                infoArea.Id = infoNode.Attributes["id"].Value;
            }
            XmlNode textnode = infoNode.SelectSingleNode("text()");
            if (textnode != null)
            {
                infoArea.Plaintext = textnode.InnerText;
            }

            return infoArea;
        }
    }
}