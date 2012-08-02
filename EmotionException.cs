using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmotionML
{
    [Serializable()]
    public class EmotionException : System.Exception
    {
        public EmotionException() : base() { }
        public EmotionException(string message) : base(message) { }
        public EmotionException(string message, System.Exception inner) : base(message, inner) { }

        protected EmotionException(System.Runtime.Serialization.StreamingContext context) { }
    }
}
