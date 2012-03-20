using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmotionML
{
    class EmotionReference
    {
        const string EMOTION_EXPRESSED_BY = "expressedBy";
        const string EMOTION_EXPERIENCED_BY = "experiencedBy";
        const string EMOTION_TRIGGERED_BY = "triggeredBy";
        const string EMOTION_TARGETED_BY = "targetedBy";

        protected Uri uri = null;
        protected string role = EMOTION_EXPRESSED_BY;
        protected string mediaType = null;

        public EmotionReference(Uri uri) {
            this.uri = uri;
        }


    }
}
