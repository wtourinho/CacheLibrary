using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheLibrary
{
    [System.AttributeUsage(System.AttributeTargets.Class) ]
    public class CacheKeysAttribute : System.Attribute
    {
        public CacheKeysAttribute()
        {
        }
    }
}