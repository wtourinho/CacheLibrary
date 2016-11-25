using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary
{
    public class InMemoryCacheItem
    {
        public string Key { get; set; }

        public int RefreshTime { get; set; }
    }
}
