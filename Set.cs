using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{
    public class Set<EmotionPart> : List<EmotionPart> 
    {
        protected Uri uri;
    
        public Uri Uri { 
            get { return uri; }
            set { uri = value; }
        }

        public Set(Uri uri) : base()
        {
            this.uri = uri;
        }

        public Set()
        {
            uri = null;
        }
    }
}
