using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Demo
{
    public class Demo2DataLayer
    {
        [AutomaticCache(Demo2CacheKeysRegister.Keys.DATA_LAYER__GET_DATA, RefreshTimeEnum.REFRESH_TIME_01MIN)]
        public static string GetData()
        {
            return DateTime.Now.ToString();
        }

        public static string GetWithParameter(string name, string lastname)
        {
            return DateTime.Now.ToString() + " " + name + " " + lastname;
        }
    }
}
