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
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{
    //OPTIMIZE: make searchable by name, so we can use emotion.Categories[categoryname].Confidence
    //OPTIMIZE: make possible Set<Part> partset = new Set<Category>(); (typs exception)
    //OPTIMIZE: add Equals
    public class Set<Part> : List<Part>
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

        /// <summary>
        /// searches entries in list by name
        /// </summary>
        /// <param name="valueName">name in the name-attribute</param>
        /// <returns>part of emotion or null if nothing found</returns>
       /* public EmotionmlLib.Part searchByName(string valueName)
        {
            int foundOnIndex = this.FindIndex(delegate(EmotionmlLib.Part controlPart)
            {
                return controlPart.Name == valueName;
            });
            if (foundOnIndex != -1)
            {
                return (EmotionmlLib.Part)this.ElementAt(foundOnIndex);
            }
            else
            {
                return null;
            }
        }*/
    }
}
