using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{   //TODO: Nachfragen bei W3C, ob <trace freq=""> nun int oder string ist. Je nachdem die Beschreibung ändern lassen.
    public class EmotionTrace
    {
        /// <summary>
        /// sampling freuquency in Hz
        /// </summary>
        protected string frequenz = "";
        /// <summary>
        /// numeric scale values from interval [0;1] (changes over time)
        /// space seperated
        /// </summary>
        protected string samples = "";

        public EmotionTrace(string frequenz, string samples) {
            if (frequenz != "")
            {
                this.frequenz = frequenz;
            }
            if (samples != "")
            {
                this.samples = samples;
            }
            //TODO: throw Exceptions
        }

        public void setFrequenz(int frequenz, string samples) {
            if (frequenz != 0 && samples != "")
            {
                this.frequenz = frequenz.ToString() + "Hz";
                this.samples = samples;
            }        
        }

        public void setFrequenz(int frequenz)
        {
            if (frequenz != 0)
            {
                this.frequenz = frequenz.ToString() + "Hz";
            }
        }

        public void setSamples(string samples)
        {
            if (samples != "")
            {
                this.samples = samples;
            }
        }

        public string getFrequenz()
        {
            return frequenz;
        }

        public string getSamples()
        {
            return samples;
        }

        public string toXml()
        {
            //FIXME: TODO
            return "";
        }
    }
}
