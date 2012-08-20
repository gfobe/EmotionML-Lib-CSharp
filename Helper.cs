using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Reflection;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Helper
    {
        private const string schemaTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <xsd:schema xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                elementFormDefault=""qualified"" attributeFormDefault=""unqualified"">
                <xsd:element name=""validating""><xsd:simpleType>
                    <xsd:restriction base=""{0}""/>
                </xsd:simpleType></xsd:element>
             </xsd:schema>
        ";

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
    }
}
