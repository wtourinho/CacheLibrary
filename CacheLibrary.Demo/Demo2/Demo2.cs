using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Demo
{
    class Demo2
    {
        public static readonly CacheLibrary.ICacheProvider CacheManager =
           new CacheLibrary.CacheProvider();

        public static void Run()
        {
            CacheManager.Init(System.Reflection.Assembly.GetExecutingAssembly());

            string p1 = "foo";
            string p2 = "bar";

            while (true)
            {
                var result = CacheManager.Execute(Demo2DataLayer.GetData, Demo2CacheKeysRegister.Items.DATA_LAYER__GET_DATA);

                var result2 = CacheManager.Execute(() =>
                {
                    return Demo1DataLayer.GetWithParameter(p1, p2);
                }, Demo2CacheKeysRegister.Items.DATA_LAYER__GET_DATA_WITH_PARAMETERS.Format(p1, p2));

                Console.WriteLine("GetData: " + result);
                Console.WriteLine("GetWithParameter: " + result2);

                Console.ReadKey();
            }
        }
    }
}
