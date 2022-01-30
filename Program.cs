using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> objectToCache = new List<string>();
            objectToCache.Add(DateTime.Now.ToString());
            objectToCache.Add(DateTime.UtcNow.ToString());

            ///Insert value in cache object
            ///Object can be cached for days,hours,Minutes,Seconds 
            CacheManager.Insert(objectToCache, "CacheTheList", 10, ConfigReader.CacheTimeStamp.Seconds, ConfigReader.SlidingExpiration);

            //Fetch value from the cache.
            var cacheObject = CacheManager.FetchValue<List<string>>("CacheTheList");

            if (cacheObject != null && cacheObject.Any())
            {
                if (cacheObject.Count == objectToCache.Count)
                {
                    Console.WriteLine("Both the objects are same");
                }
                else
                {
                    Console.WriteLine("Object's are diffrent");
                }
            }

            CacheManager.Clear("CacheTheList");

            //Fetch value from the cache.
            var cacheObjectAfterClear = CacheManager.FetchValue<List<string>>("CacheTheList");

            if (cacheObjectAfterClear != null && cacheObjectAfterClear.Any())
            {
                if (cacheObjectAfterClear.Count == objectToCache.Count)
                {
                    Console.WriteLine("Both the objects are same");
                }
                else
                {
                    Console.WriteLine("Object's are diffrent");
                }
            }
            else
            {
                Console.WriteLine("No Object Found in cache.Cache Object is cleared!!!!");
            }
        }
    }
}
