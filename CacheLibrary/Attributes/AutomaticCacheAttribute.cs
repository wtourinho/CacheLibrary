using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheLibrary
{
    [System.AttributeUsage(System.AttributeTargets.Method) ]
    public class AutomaticCacheAttribute : System.Attribute
    {
        public string Key { get; set; }

        public int RefreshTime { get; set; }

        public AutomaticCacheAttribute(string key, int refreshTime)
        {
            this.Key = key;
            this.RefreshTime = refreshTime;
        }
    }
}