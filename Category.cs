using System;
using System.Collections.Generic;
using System.Linq;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Category : Part
    {
        /// <summary>
        /// Vocabularies for categories of EmotionML 1.0 out of http://www.w3.org/TR/emotion-voc/xml
        /// </summary>
        public const string CATEGORY_BIG6 = "http://www.w3.org/TR/emotion-voc/xml#big6";
        public const string CATEGORY_EVERYDAY = "http://www.w3.org/TR/emotion-voc/xml#everyday-categories";
        public const string CATEGORY_OCC = "http://www.w3.org/TR/emotion-voc/xml#occ-categories";
        public const string CATEGORY_FSRE = "http://www.w3.org/TR/emotion-voc/xml#fsre-categories";
        public const string CATEGORY_FRIJDA = "http://www.w3.org/TR/emotion-voc/xml#frijda-categories";

        public Category(string name) : base(name)
        {}

        public Category(string name, float? value) : base(name, value)
        {}

        public Category(string name, float? value, float? confidence) : base (name, value, confidence)
        {}
    }
}