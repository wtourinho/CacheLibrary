using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary
{
    public static class GenerateCacheKey
    {
        public static CacheKeyItem Format(this CacheKeyItem value, params string[] parameters)
        {
            var executionItem = new CacheKeyItem(value.Key, value.RefreshTime);
            
            if (parameters == null || parameters.Length == 0)
            {
                return executionItem;
            }

            for(var i = 0; i < parameters.Length; i++)
            {
                executionItem.Key = executionItem.Key + "|" + (parameters[i] ?? "null");
            }

            return executionItem;
        }
    }
}
