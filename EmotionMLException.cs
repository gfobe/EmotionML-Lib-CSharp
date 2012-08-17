using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{
    [Serializable()]
    public class EmotionMLException : System.Exception
    {
        public EmotionMLException() : base() { }
        public EmotionMLException(string message) : base(message) { }
        public EmotionMLException(string message, System.Exception inner) : base(message, inner) { }

        protected EmotionMLException(System.Runtime.Serialization.StreamingContext context) { }
    }
}
