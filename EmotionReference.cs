using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{
    class EmotionReference
    {
        const string EXPRESSED_BY = "expressedBy";
        const string EXPERIENCED_BY = "experiencedBy";
        const string TRIGGERED_BY = "triggeredBy";
        const string TARGETED_BY = "targetedBy";

        protected Uri uri = null;
        protected string role = null;
        protected string mediaType = null;  //TODO: validate media type

        public EmotionReference(Uri uri) {
            this.uri = uri;
        }

        public string getRole()
        {
            if(role != null) {
                return role;
            } else {
                return EXPRESSED_BY;
            }
        }

        public void setRole(string newRole)
        {
                role = newRole;            
        }

        public Uri getUri()
        {
            return uri;
        }

        public void setUri(Uri newUri)
        {
            uri = newUri;
        }

        public void setUri(string stringUri)
        {
            Uri newUri = new Uri(stringUri);
            setUri(newUri);
        }

        public string toXml()
        {
            //FIXME: TODO
            return "";
        }

    }
}
