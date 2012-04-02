using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmotionML
{
    public class EmotionSet<EmotionPart> : List<EmotionPart> 
    {
        protected Uri uri = null;
        public Uri getEmotionsetUri()
        {
            return uri;
        }

        public EmotionSet(Uri uri)
        {
            //TODO: übergeordneten Konstruktor aufrufen
            setEmotionsetUri(uri);
        }

        public void setEmotionsetUri(Uri uri)
        {
            this.uri = uri;
        }
    }
}
