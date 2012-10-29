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

namespace Vsr.Hawaii.EmotionmlLib
{
    //OPTIMIZE: make searchable by name, so we can use emotion.Categories[categoryname].Confidence
    //OPTIMIZE: make possible Set<Part> partset = new Set<Category>(); (typs exception)
    //TODO: add Equals
    public class Set<T> : List<Part>
    {
        /// <summary>
        /// set URI
        /// </summary>
        protected Uri uri;
        /// <summary>
        /// vocabulary with Items of set
        /// </summary>
        protected Vocabulary vocabulary = null;

    
        public Uri Uri 
        { 
            get { return uri; }
            set { uri = value; }
        }

        public Vocabulary Vocabulary
        {
            get { return vocabulary; }
        }


        public Set(Uri uri) : base()
        {
            //TODO: rise exception when URI is empty
            this.uri = uri;
        }

        public Set() : base()
        {
            uri = null;
        }

        /// <summary>
        /// adds item to list
        /// </summary>
        /// <param name="part">part to add to list</param>
        public new void Add(Part part) {
            if (vocabulary != null)
            {
                if (isInVocabulary(new Item(part.Name)))
                {
                    base.Add(part);
                }
            }
        }

        /// <summary>
        /// looks if a item can be stored in this set
        /// </summary>
        /// <param name="item">item to test if it can be stored</param>
        /// <returns>item can be stored in set</returns>
        public bool isInVocabulary(Item item)
        {
            if (null == vocabulary)
            {
                return false;
            }

            foreach (Item testItem in vocabulary.Items)
            {
                if (item.Name == testItem.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// searches entries in list by name
        /// </summary>
        /// <param name="valueName">name in the name-attribute</param>
        /// <returns>part of emotion or null if nothing found</returns>
        public Part searchByName(string valueName)
        {
            int foundOnIndex = this.FindIndex(delegate(Part controlPart)
            {
                return controlPart.Name == valueName;
            });
            if (foundOnIndex != -1)
            {
                return (Part)this.ElementAt(foundOnIndex);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// load vocabulary for set
        /// </summary>
        protected void loadVocabulary()
        {
            string vocabularyName = uri.Fragment;
            string vocabularyUrl = uri.Scheme + ':' + uri.Host + uri.AbsolutePath;
            List<Vocabulary> vocabularies = HelperVocabularycheck.loadVocabularyListFromUrl(new Uri(vocabularyUrl));

            foreach (Vocabulary vocabulary in vocabularies)
            {
                if (vocabularyName == vocabulary.Id)
                {
                    this.vocabulary = vocabulary;
                    return;
                }
            }

            throw new EmotionMLException('"' + vocabularyName + "\" is not in vocabulary file \"" + vocabularyUrl + '"');
        }
    }
}
