using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Vsr.Hawaii.EmotionmlLib
{
    //TODO: remove public
    public class HelperMimetype
    {
        /// <summary>
        /// cache helper for MIME type list
        /// </summary>
        public static HelperCache mimeTypeCache = null;

        public const string CACHE_CODE = "mimetypes";

        /// <summary>
        /// checks the given MIME type against the official MIME type list of IANA
        /// </summary>
        /// <see cref="http://www.iana.org/assignments/media-types/index.html"/>
        public static bool crawlLiveMimetypes = true;

        /// <summary>
        /// checks if string can be a valid MIME type
        /// deprecated and obsolete MIME types returns false
        /// </summary>
        /// <param name="mediaTypeString">possible MIME-Type</param>
        /// <returns>given can be a MIME-Type</returns>
        public static bool isMediaType(string mediaTypeString)
        {
            /* 1. check if it can be a MIME type
             * 2. check if it is a experimental one
             * 3. search in cache for IANA MIME type list
             * 4. crawl and generate live IANA MIME type list
             * 5. if we can/should not crawl, use local MIME type file as fallback
             * 6. check MIME type
             */

            // init
            XmlDocument ianaMimeTypeXml = null;
            if(mimeTypeCache == null) {
                mimeTypeCache = new HelperCache();
            }

            // 1. check if it can be a MIME type
            Regex mimetypeRegEx = new Regex("^[a-z0-9\\-+]+\\/[a-zA-Z0-9\\-\\+\\._]+$");

            if (!mimetypeRegEx.IsMatch(mediaTypeString))
            {
                return false; //not in format type/subtype
            }

            string[] parts = mediaTypeString.ToLower().Split('/');

            // 2. check if it is a experimental one
            if ("example" == parts[0] || "x-" == parts[0].Substring(0, 2) || "X-" == parts[0].Substring(0, 2))
            {
                return true;
            }

            // 3. search in cache for IANA MIME type list
            object returnObject = mimeTypeCache.getItem(CACHE_CODE);
            if(null != returnObject) {
                ianaMimeTypeXml = (XmlDocument)returnObject;
            }

            // 4. crawl and generate live IANA MIME type list
            if (null == ianaMimeTypeXml && crawlLiveMimetypes)
            {
                /*try
                {*/
                    ianaMimeTypeXml = crawlIanaLiveMimetypes();
                    mimeTypeCache.storeItem(CACHE_CODE, ianaMimeTypeXml);
                /*} catch {
                    //ignore error, we will use fallback in next step
                    //OPTIMIZE: add a logging component
                }*/
            }

            // 5. if we can/should not crawl, use local MIME type file as fallback
            if (null == ianaMimeTypeXml)
            {
                ianaMimeTypeXml = new XmlDocument();
                ianaMimeTypeXml.LoadXml(Helper.loadInternalResource("mime-types.xml"));
                mimeTypeCache.storeItem(CACHE_CODE, ianaMimeTypeXml);
            }

            // 6. check MIME type
            XmlNodeList types = ianaMimeTypeXml.SelectNodes("/*/*");
            foreach (XmlNode type in types)
            {
                if (parts[0] == type.Name) //application, audio, video...
                {
                    //we found the right type
                    // 2. check if it is a experimental one
                    if ("x-" == parts[1].Substring(0, 2) || "X-" == parts[1].Substring(0, 2))
                    {
                        return true;
                    }

                    // search in official list
                    XmlNodeList subtypes = type.SelectNodes("./*/child::text()");
                    foreach (XmlNode subtype in subtypes)
                    {
                        if (parts[1] == subtype.InnerText)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// crawls the current list of MIME types from IANA
        /// deprecated and obsolete entries are not crawled
        /// internet connection needed
        /// </summary>
        /// <see cref="http://www.iana.org/assignments/media-types/index.html"/>
        /// <returns>proprietary XML document with mime types</returns>
        public static XmlDocument crawlIanaLiveMimetypes()
        {
            //TODO: find a machine readable official list of IANA
            //<mime-types><mainType>subType</mainType></mime-types>
            XmlDocument xml = new XmlDocument();
            XmlElement mimeTypes = xml.CreateElement("mime-types");

            // access main page to get main types
            string mainpage = Helper.doGetRequest(new Uri("http://www.iana.org/assignments/media-types/index.html"));
            //<p><a href="/assignments/media-types/application/">application</a></p>
            Regex maintypeRegex = new Regex("href=\".*/media-types/([a-z0-9]+)/.+?>");
            MatchCollection maintypeMatches = maintypeRegex.Matches(mainpage);
            foreach (Match match in maintypeMatches)
            {
                string mainTypeName = match.Groups[1].Value;
                //on examples they have another link structure
                if ("examples" == mainTypeName)
                {
                    mainTypeName = "example";
                }
                XmlElement mainType = xml.CreateElement(mainTypeName);

                // search for subtypes of this main type
                string subpage = Helper.doGetRequest(new Uri("http://www.iana.org/assignments/media-types/" + mainTypeName + "/"));
                
                //<td>&nbsp;</td>
                //<td><a>1d-interleaved-parityfec</a></td>
                Regex subtypeRegex = new Regex("<td>(&nbsp;|\\s)*</td>\\s*<td>(<.*?>|\\s)*([a-zA-Z0-9\\-\\+\\._]+)(</.*?>|\\s)*</td>");
               
                MatchCollection subtypeMatches = subtypeRegex.Matches(subpage);
                foreach (Match submatch in subtypeMatches)
                {
                    string subTypeName = submatch.Groups[3].Value;

                    XmlElement newType = xml.CreateElement("subtype");
                    newType.InnerText = subTypeName;
                    mainType.AppendChild(newType);
                }
                mimeTypes.AppendChild(mainType);
            }

            xml.AppendChild(mimeTypes);
            return xml;
        }
    }
}
