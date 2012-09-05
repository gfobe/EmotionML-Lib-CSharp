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

namespace Vsr.Hawaii.EmotionmlLib
{

    public class EmotionML
    {
        /// <summary>
        /// Namespace for EmotionML 1.0
        /// </summary>
        public const string NAMESPACE = "http://www.w3.org/2009/10/emotionml";

        /// <summary>
        /// recommented XML-prefix of EmotionML
        /// </summary>
        public const string PRAEFIX = "emo";

        /// <summary>
        /// MIME type of EmotionML
        /// </summary>
        public const string MIME_TYPE = "application/emotionml+xml";

        /// <summary>
        /// File-extension of EmotionML
        /// </summary>
        public const string FILE_EXTENSION = "emotionml";

        /// <summary>
        /// Version of highest EmotionML-Recommendation
        /// </summary>
        public const string VERSION = "1.0";

        /// <summary>
        /// Versions of all EmotionML Recommendations
        /// </summary>
        public static readonly string[] VERSIONS = new string[] { "1.0" };

        /// <summary>
        /// Version of this EmotionML-Library
        /// </summary>
        public const string LIBRARY_VERSION = "1.0alpha";

        /// <summary>
        /// Version of highest implemented EmotionML-Recommendation
        /// </summary>
        public const string LIBRARY_EMOTIONML_VERSION = "1.0 - Candidate Recommendation 2012-05-10";

        //TODO: XML-Schemata vom W3C
        //http://www.w3.org/TR/2012/CR-emotionml-20120510/emotionml.xsd
        //http://www.w3.org/TR/2012/CR-emotionml-20120510/emotionml-fragments.xsd
    }
}