using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InMemoryCaching
{
    public class ConfigReader
    {
        public enum CacheTimeStamp
        {
            Seconds,
            Minutes,
            Hours,
            Days,
            Months,
        }

        public const string AbsoluteExpiration = "AbsoluteExpiration";
        public const string SlidingExpiration = "SlidingExpiration";
    }
}
