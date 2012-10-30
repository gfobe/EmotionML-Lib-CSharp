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
using System.Text;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Vocabulary
    {
        /// <summary>
        /// type of vocabulary (category, dimension, appraisal, action-tendency)
        /// </summary>
        protected string type;
        /// <summary>
        /// xml:ID of vocabulary
        /// </summary>
        protected string id;
        /// <summary>
        /// items in vocabulary
        /// </summary>
        protected List<Item> items = new List<Item>();
        /// <summary>
        /// <info/> for vocabulary
        /// </summary>
        protected Info info = null;

        public Vocabulary(string type, string id, Item item) 
        {
            Id = id;
            Type = type;
            addItem(item);
        }

        public string Id
        {
            get { return id; }
            set
            {
                if (value == "")
                {
                    throw new EmotionMLException("Emotion vocabulary must have an id.");
                }
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

        public string Type
        {
            get { return type; }
            set
            {
                switch (value)
                {
                    case Part.CATEGORY:
                    case Part.DIMENSION:
                    case Part.APPRAISAL:
                    case Part.ACTIONTENDENCY:
                        break;
                    default:
                        throw new EmotionMLException(
                            "Emotion vocabulary must have a type like " +
                            Part.CATEGORY + ", " +
                            Part.DIMENSION + ", " +
                            Part.APPRAISAL + ", " +
                            Part.ACTIONTENDENCY + "."
                        );
                }

                type = value;
            }

        }
             
        public Info Info {
            get { return info; }
            set { info = value; }
        }

        public List<Item> Items {
            get { return items; }
        }


        /// <summary>
        /// compares this vocabulary with another for equality
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>objects are equal</returns>
        public override bool Equals(object obj)
        {
            string[] ignore = new string[] { };
            return Equals(obj, ignore);
        }

        /// <summary>
        /// compares this vocabulary with another for equality
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <param name="ignore">ignorations (supported: info)</param>
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

            Vocabulary control = (Vocabulary)obj;
            if (!ignore.Contains<string>("info"))
            {
                if (!this.info.Equals(control.Info))
                {
                    return false;
                }
            }
            if (this.id == control.Id
            && this.type == control.Type)
            {
                //iterate througt items
                List<Item> controlItems = control.Items;
                if (this.items.Count != controlItems.Count)
                {
                    return false; //not same numer of items
                }

                foreach (Item testItem in this.items)
                {
                    bool continueIteration = false;
                    foreach (Item testControlItem in controlItems)
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

                return true; //all items found in items of control object
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// adds item to vokabulary
        /// </summary>
        /// <param name="name">name of item</param>
        public void addItem(Item newItem) 
        {
            if (!items.Exists(delegate(Item existingItem)
            {
                return existingItem.Name == newItem.Name;
            }) )
            {
                items.Add(newItem);
            }
            else
            {
                throw new EmotionMLException("item \"" + newItem.Name + "\" already exists.");
            }   
        }

        /// <summary>
        /// removes item from vokabulary
        /// </summary>
        /// <param name="name">name of item</param>
        public void removeItem(Item itemToRemove)
        {
            //do not remove the last one
            if (items.Count == 1)
            {
                throw new EmotionMLException("Try to remove last item. At lest one item must be in the vocabulary.");
            }

            if (!items.Exists(delegate(Item existingItem) {
                return itemToRemove.Name == existingItem.Name;
            }))
            {
                throw new EmotionMLException("item \"" + itemToRemove.Name + "\" is not in vocabulary.");
            }
            int index = items.FindIndex(delegate(Item existingItem) {
                return existingItem.Name == itemToRemove.Name;
            });
            items.RemoveAt(index);
        }

        /// <summary>
        /// creates a DOM of Emotion
        /// </summary>
        /// <returns>DOM of emotion definition</returns>
        public XmlDocument ToDom()
        {
            XmlDocument vocabularyXml = new XmlDocument();

            //set up <vocabulary>
            XmlElement vocabulary = vocabularyXml.CreateElement("vocabulary");
            vocabulary.SetAttribute("type", this.type);
            vocabulary.SetAttribute("id", this.id);
            if (info != null)
            {
                XmlNode importedNode = vocabularyXml.ImportNode(info.ToDom().FirstChild, true);
                vocabulary.AppendChild(importedNode);
            }

            foreach (Item item in this.items)
            {
                XmlNode importedNode = vocabularyXml.ImportNode(item.ToDom().FirstChild, true);
                vocabulary.AppendChild(importedNode);
            }

            vocabularyXml.AppendChild(vocabulary);

            return vocabularyXml;
        }

        /// <summary>
        /// generates the XML for vokabulary
        /// </summary>
        /// <returns>XML</returns>
        public string ToXml()
        {
            return ToDom().OuterXml;
        }
    }
}
