using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching
{
    public static class CacheManager
    {
        static MemoryCache GetDefaultCache()
        {
            return MemoryCache.Default;
        }

        /// <summary>
        /// Fetches the cahced value. Returns null if no object is found in cache
        /// </summary>
        /// <param name="cacheKey">Cache key for whihc we need to fetch the data</param>
        /// <returns>The cahced object. Returns null if no object is found in cache</returns>                
        public static T FetchValue<T>(string cacheKey)
        {
            var cache = GetDefaultCache();

            if (string.IsNullOrEmpty(cacheKey) && cache.Get(cacheKey) == null)
            {
                return default(T);
            }

            return (T)cache.Get(cacheKey);
        }

        /// <summary>
        /// Inserts the object in the cache
        /// </summary>
        /// <param name="obj">The object to cache.</param>                      
        /// <param name="cacheKey">cache key for whihc we need to insert tha data in cache.</param>
        /// <param name="cacheDuration">Time duration for which need to cache data.</param>
        /// <param name="timeStamp">Time stamp for which we need to do caching.days,hours</param>
        /// <param name="cachingType"> Type of the caching AbsoluteExpiration/SlidingExpiration.</param>
        public static void Insert<T>(T obj, string cacheKey, int cacheDuration, ConfigReader.CacheTimeStamp timeStamp, string cachingType)
        {
            var cache = GetDefaultCache();
            var cachePolicty = new CacheItemPolicy();

            if (!string.IsNullOrEmpty(cacheKey) && obj != null)
            {
                switch (timeStamp)
                {
                    default:
                    case ConfigReader.CacheTimeStamp.Seconds:
                        if (cachingType == ConfigReader.AbsoluteExpiration)
                        {
                            cachePolicty.AbsoluteExpiration = DateTime.Now.AddSeconds(cacheDuration);
                        }
                        else
                        {
                            cachePolicty.SlidingExpiration = TimeSpan.FromSeconds(cacheDuration);
                        }
                        break;

                    case ConfigReader.CacheTimeStamp.Minutes:
                        if (cachingType == ConfigReader.AbsoluteExpiration)
                        {
                            cachePolicty.AbsoluteExpiration = DateTime.Now.AddMinutes(cacheDuration);
                        }
                        else
                        {
                            cachePolicty.SlidingExpiration = TimeSpan.FromMinutes(cacheDuration);
                        }
                        break;

                    case ConfigReader.CacheTimeStamp.Hours:
                        if (cachingType == ConfigReader.AbsoluteExpiration)
                        {
                            cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(cacheDuration);
                        }
                        else
                        {
                            cachePolicty.SlidingExpiration = TimeSpan.FromHours(cacheDuration);
                        }
                        break;

                    case ConfigReader.CacheTimeStamp.Days:
                        if (cachingType == ConfigReader.AbsoluteExpiration)
                        {
                            cachePolicty.AbsoluteExpiration = DateTime.Now.AddDays(cacheDuration);
                        }
                        else
                        {
                            cachePolicty.SlidingExpiration = TimeSpan.FromDays(cacheDuration);
                        }
                        break;

                    case ConfigReader.CacheTimeStamp.Months:
                        cachePolicty.AbsoluteExpiration = DateTime.Now.AddMonths(cacheDuration);
                        break;
                }

                cache.Add(cacheKey, obj, cachePolicty);
            }
        }

        /// <summary>
        /// Removes the object from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key for which we need to remove cached data.</param>
        public static void Clear(string cacheKey)
        {
            var cache = GetDefaultCache();
            if (cache != null && cache.Contains(cacheKey))
            {
                cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Update the cached object by adding paraeters.
        /// </summary>
        /// <param name="obj">The object to cache.</param>                      
        /// <param name="cacheKey">cache key for whihc we need to insert tha data in cache.</param>
        /// <param name="cacheDuration">Time duration for which need to cache data.</param>
        /// <param name="timeStamp">Time stamp for which we need to do caching.days,hours</param>
        /// <param name="cachingType">Inticates the type of caching used SlidingExpiration/AbsoluteExpiration.</param>
        /// <param name="parameters">Indicates the parameter whihc we need to add in a list.</param>
        public static void UpdateCache<T>(List<T> obj, string cacheKey, int cacheDuration, ConfigReader.CacheTimeStamp timeStamp, string cachingType, T parameters)
        {
            List<T> cachedList = new List<T>();
            try
            {
                var result = FetchValue<T>(cacheKey);

                if (result != null)
                {
                    cachedList.Add(result);
                }

                if (cachedList != null && cachedList.Count > 0)
                {
                    cachedList.Add(parameters);

                    Insert(obj,
                            cacheKey,
                            cacheDuration,
                            timeStamp,
                            cachingType);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

}
