using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary
{
    public class CacheKeyItem
    {
        public CacheKeyItem(string key, int refreshTime)
        {
            this.Key = key;
            this.RefreshTime = refreshTime;
        }
        public string Key { get; set; }

        public int RefreshTime { get; set; }
    }
}
