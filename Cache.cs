using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vsr.Hawaii.EmotionmlLib
{
    class Cache
    {
        /// <summary>
        /// default cachetime in seconds: 1 day (24 * 60 * 60)
        /// </summary>
        public const long DEFAULT_CACHETIME = 86400;

        /// <summary>
        /// data of cache: key => data
        /// </summary>
        protected SortedList<string, object> cacheData = new SortedList<string, object>();
        /// <summary>
        /// cache livetime: key => cache invalidate after this time
        /// nothing exists = no lifetime end
        /// </summary>
        protected SortedList<string, DateTime> cacheLifetime = new SortedList<string, DateTime>();

        /// <summary>
        /// is cache is in time
        /// </summary>
        /// <param name="key">cache key</param>
        /// <returns>cache is valid</returns>
        protected bool isValid(string key)
        {
            if (!cacheLifetime.Keys.Contains(key))
            {
                return true; //cache has no end of life
            }
            else if(DateTime.Now < cacheLifetime[key])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// stores a object in cache
        /// </summary>
        /// <param name="key">key for item, to access it later</param>
        /// <param name="cacheObject">the object to cache</param>
        /// <param name="lifetime">lifetime of cache in seconds; null = endless life</param>
        public void storeItem(string key, object cacheObject, long? lifetime = DEFAULT_CACHETIME)
        {
            //store data
            if (cacheData.Keys.Contains(key))
            {
                cacheData[key] = cacheObject;
            }
            else
            {
                cacheData.Add(key, cacheObject);
            }

            //store lifetime
            if (lifetime != null)
            {
                DateTime endTime = DateTime.Now.AddSeconds((double)lifetime);

                if (cacheLifetime.Keys.Contains(key))
                {
                    cacheLifetime[key] = endTime;
                }
                else
                {
                    cacheLifetime.Add(key, endTime);
                }
            }
        }

        /// <summary>
        /// reads an entry from the cache
        /// </summary>
        /// <param name="key">key for item</param>
        /// <returns>object stored in cache or null, if cache lifetime reached or object is not in cache</returns>
        public object getItem(string key)
        {
            if(cacheData.Keys.Contains(key)) {
                if (isValid(key))
                {
                    return cacheData[key];
                }
                else
                {
                    flushCache(key);
                    return null;
                }
            } else {
                return null;
            }
        }

        /// <summary>
        /// flush cache of item
        /// </summary>
        /// <param name="key">key of item in cache</param>
        public void flushCache(string key)
        {
            if (cacheData.Keys.Contains(key))
            {
                cacheData.Remove(key);
            }
            if (cacheLifetime.Keys.Contains(key))
            {
                cacheLifetime.Remove(key);
            }
        }

        /// <summary>
        /// flush whole cache
        /// </summary>
        public void flushCache()
        {
            foreach (string key in cacheData.Keys)
            {
                cacheData.Remove(key);
            }
            foreach (string key in cacheLifetime.Keys)
            {
                cacheLifetime.Remove(key);
            }
        }
    }
}