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
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Helper
    {
        /// <summary>
        /// XML Schema template to validate some special types
        /// </summary>
        private const string schemaTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xsd:schema xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                elementFormDefault=""qualified"" attributeFormDefault=""unqualified"">
                <xsd:element name=""validating""><xsd:simpleType>
                    <xsd:restriction base=""{0}""/>
                </xsd:simpleType></xsd:element>
             </xsd:schema>
        ";

        /// <summary>
        /// XML template to validate some special things
        /// </summary>
        private const string xmlTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <validating>{0}</validating>
        ";


        /// <summary>
        /// validate a string against xsd:ID
        /// </summary>
        /// <param name="id">ID to validate</param>
        /// <returns>ID is a xsd:ID</returns>
        public static bool isXsdId(string id)
        {
            //OPTIMIZE: do it a better way http://www.w3.org/TR/1999/REC-xml-names-19990114/#NT-NCName
            string xmlString = String.Format(xmlTemplate, id);
            string schemaString = String.Format(schemaTemplate, "xsd:ID");

            return Helper.validateAgainstSchema(xmlString, schemaString);
        }

        /// <summary>
        /// validate a string against xsd:anyURI
        /// </summary>
        /// <param name="id">URI to validate</param>
        /// <returns>URI is a xsd:anyURI</returns>
        public static bool isXsdAnyUri(string uri)
        {
            //OPTIMIZE: do it a better way
            string xmlString = String.Format(xmlTemplate, uri);
            string schemaString = String.Format(schemaTemplate, "xsd:anyURI");

            return Helper.validateAgainstSchema(xmlString, schemaString);
        }

        /// <summary>
        /// validate a string against xsd:nmtokens
        /// </summary>
        /// <param name="id">token to validate</param>
        /// <returns>token is a xsd:nmtokens</returns>
        public static bool isNmtokens(string token)
        {
            //FIXME: Der Typ 'http://www.w3.org/2001/XMLSchema:nmtokens' ist kein einfacher Typ oder wurde nicht deklariert.

            //OPTIMIZE: do it a better way
            string xmlString = String.Format(xmlTemplate, token);
            string schemaString = String.Format(schemaTemplate, "xsd:NMTOKENS");

            return Helper.validateAgainstSchema(xmlString, schemaString);
        }

        /// <summary>
        /// validates a XML against a XMLSchema
        /// </summary>
        /// <param name="xmlString">the XML</param>
        /// <param name="schemaString">the Schema</param>
        /// <returns>XML ist valid against XMLSchema</returns>
        public static bool validateAgainstSchema(string xmlString, string schemaString)
        {
            StringReader stringreader = new StringReader(schemaString);
            XmlSchema schema = XmlSchema.Read(stringreader, null);
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            xml.Schemas.Add(schema);
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

        /// <summary>
        /// loads an internal schema
        /// supported schemata: emotionml.xsd, emotionml-fragments.xsd
        /// </summary>
        /// <param name="resourceName">internal name of the resource</param>
        /// <returns>content of resource</returns>
        public static string loadInternalResource(string resourceName) {
            //generate resource name
            Type mytype = typeof(Helper);
            string myNamespace = mytype.Namespace;

            string ressourceToLoad = myNamespace + ".resources." + resourceName;

            //load resource
            Assembly ownAssembly = Assembly.GetExecutingAssembly();
            string[] assemblyRessources = ownAssembly.GetManifestResourceNames();

            if (!assemblyRessources.Contains<string>(ressourceToLoad))
            {
                throw new EmotionMLException("Resource \"" + resourceName + "\" is not a internal resource of this library.");
            }
            
            System.IO.Stream file = ownAssembly.GetManifestResourceStream(ressourceToLoad);

            //return it as normal string
            StreamReader reader = new StreamReader(file);
            string resourceContent = reader.ReadToEnd();

            return resourceContent;
        }

        /// <summary>
        /// converts double to string without localisation
        /// </summary>
        /// <param name="number">double</param>
        /// <returns>string</returns>
        public static string double2string(double number)
        {
            if (null == number)
            {
                return null;
            }

            return number.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// support null for double2string()
        /// </summary>
        /// <param name="number">double</param>
        /// <returns>string</returns>
        public static string double2string(double? number)
        {
            if (null == number)
            {
                return null;
            }
            else
            {
                return double2string((double)number);
            }
        }

        /// <summary>
        /// converts a string to double without localisation
        /// </summary>
        /// <param name="number">string</param>
        /// <returns>double</returns>
        public static double string2double(string number)
        {
            return double.Parse(number, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
        }

        /// <summary>
        /// checks if string can be a valid MIME-Type
        /// </summary>
        /// <param name="mediaTypeString">possible MIME-Type</param>
        /// <returns>given can be a MIME-Type</returns>
        public static bool isMediaType(string mediaTypeString)
        {
            Regex mimetypeRegEx = new Regex("^[a-z\\-+]+\\/[a-z\\-+]+$");

            mediaTypeString = mediaTypeString.ToLower();

            if (!mimetypeRegEx.IsMatch(mediaTypeString))
            {
                return false; //not in format type/subtype
            }
            
            string[] parts = mediaTypeString.ToLower().Split('/');

            // experimental type
            if("x-" == parts[0].Substring(0,2)) {
                return true;
            }
            // some registered type
            switch (parts[0])
            {
                case "example":
                    return true; //subtype does not matter
                case "application":
                case "audio":
                case "image":
                case "message":
                case "model":
                case "multipart":
                case "text":
                case "video":
                    // experimental subtype
                    if ("x-" == parts[1].Substring(0, 2))
                    {
                        return true;
                    }
                    //OPTIMIZE: check all the subtypes - but how?
                    return true;
            }

            return false;
        }
    }
}
