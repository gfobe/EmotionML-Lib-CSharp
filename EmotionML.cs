using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{

    public class EmotionML
    {
        /// <summary>
        /// Namespace for EmotionML 1.0
        /// </summary>
        public const string NAMESPACE = "http://www.w3.org/2009/10/emotionml";

        /// <summary>
        /// XML-praefix of EmotionML
        /// </summary>
        public const string PRAEFIX = "emo";

        /// <summary>
        /// MIME media type of EmotionML
        /// </summary>
        public const string MIME_TYPE = "application/emotionml+xml";

        /// <summary>
        /// File-extension of EmotionML
        /// </summary>
        public const string FILE_EXTENSION = "emotionml";

        /// <summary>
        /// Version of highest implemented EmotionML-Recommendation
        /// </summary>
        public const string VERSION = "1.0 - Candidate Recommendation 2012-05-10";

        /// <summary>
        /// Version of this EmotionML-Library
        /// </summary>
        public const string LIBRARY_VERSION = "1.0alpha";

        //TODO: XML-Schemata vom W3C
        //http://www.w3.org/TR/2012/CR-emotionml-20120510/emotionml.xsd
        //http://www.w3.org/TR/2012/CR-emotionml-20120510/emotionml-fragments.xsd
    }
}