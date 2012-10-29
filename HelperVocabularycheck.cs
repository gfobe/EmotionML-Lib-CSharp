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
using System.Net;
using System.IO;

namespace Vsr.Hawaii.EmotionmlLib
{
    class HelperVocabularycheck
    {
        /// <summary>
        /// User agent given, when some other EmotionML (e.g. vocabulary) is crawled with this library
        /// </summary>
        public const string USER_AGENT = "EmotionML Library for C# v" + EmotionML.LIBRARY_VERSION;

        /// <summary>
        /// enables caching when loading a vocabulary by URL (default = true)
        /// </summary>
        public static bool enableCaching = true;

        /// <summary>
        /// cache for already readed vocabularies List<uri, vocabulary>
        /// </summary>
        private static SortedList<string,List<Vocabulary>> vocabularyCache = new SortedList<string,List<Vocabulary>>();
        

        /// <summary>
        /// load a vocabulary list from a URL
        /// </summary>
        /// <param name="uri">URI for emotionml vocabulary ressource</param>
        /// <returns>list of emotionml vocabularies</returns>
        public static List<Vocabulary> loadVocabularyListFromUrl(Uri uri) {
            string emotionmlString = null;
            List<Vocabulary> vocabularies = null;

            //read from cache
            //OPTIMIZE: use memory cache with garbage collection: http://msdn.microsoft.com/de-de/library/system.runtime.caching.memorycache.aspx
            if (vocabularyCache.Keys.Contains(uri.AbsoluteUri))
            {
                return vocabularyCache[uri.AbsoluteUri];
            }

            //normal processing
            if ("http://www.w3.org/TR/emotion-voc/xml" == uri.AbsoluteUri)
            {
                //use internal ressource if we use the W3C example
                //OPTIMIZE: how to deal between changes in W3C-list and easy to use with internal ressource -> @see cache?
                emotionmlString = Helper.loadInternalResource("vocabularies.emotionml");
            }
            else
            {
                //init request
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

                //define headers
                httpWebRequest.Method = "GET";
                httpWebRequest.MediaType = "HTTP/1.1";
                httpWebRequest.UserAgent = USER_AGENT;
                httpWebRequest.Headers.Add("Accept", EmotionML.MIME_TYPE); //http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html

                //send request
                HttpWebResponse httpWebesponse = (HttpWebResponse)httpWebRequest.GetResponse();

                //receive response as UTF-8 string
                Stream dataStream = httpWebesponse.GetResponseStream();
                StreamReader streamreader = new StreamReader(dataStream, Encoding.UTF8);
                emotionmlString = streamreader.ReadToEnd();
                streamreader.Close();
            }

            //load vocabularies
            Parser parser = new Parser(emotionmlString);
            vocabularies = parser.getVocabularies();

            //store it in cache:
            if (enableCaching)
            {
                if (vocabularyCache.ContainsKey(uri.AbsoluteUri))
                {
                    vocabularyCache[uri.AbsoluteUri] = vocabularies;
                }
                else
                {
                    vocabularyCache.Add(uri.AbsoluteUri, vocabularies);
                }
            }

            return vocabularies;
        }

        /// <summary>
        /// flushes the cache dealing with loaded vocabularies
        /// </summary>
        public static void flushCache()
        {
            vocabularyCache = new SortedList<string, List<Vocabulary>>();
        }
    }
}
