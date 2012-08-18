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
            //TODO: rise exception when URI is empty
            this.uri = uri;
        }

        public Set() : base()
        {
            uri = null;
        }
    }
}
