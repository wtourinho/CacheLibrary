using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Demo
{
    class Demo2CacheKeysRegister
    {
        [CacheKeys]
        public class Keys
        {
            public const string DATA_LAYER__GET_DATA = "DataLayer.GetData";
            public const string DATA_LAYER__GET_DATA_WITH_PARAMETERS = "DataLayer.GetDataWithParameters";
        }

        public class Items
        {
            public static readonly CacheKeyItem DATA_LAYER__GET_DATA =
                new CacheKeyItem(Demo2CacheKeysRegister.Keys.DATA_LAYER__GET_DATA, RefreshTimeEnum.REFRESH_TIME_01MIN);

            public static readonly CacheKeyItem DATA_LAYER__GET_DATA_WITH_PARAMETERS =
                new CacheKeyItem(Demo2CacheKeysRegister.Keys.DATA_LAYER__GET_DATA_WITH_PARAMETERS, RefreshTimeEnum.REFRESH_TIME_01MIN);
        }
    }
}
