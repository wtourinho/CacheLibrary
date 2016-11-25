using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CacheLibrary
{
    public class CacheAutomaticRegisterItem : InMemoryCacheItem
    {
        public MethodInfo Method { get; set; }

        public DateTime NextExecutionTime { get; set; }

        public DateTime CurrentExecutionTime { get; set; }
    }
}
