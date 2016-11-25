using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Demo
{
    public class Demo1
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
                var result = CacheManager.Execute(Demo1DataLayer.GetData, "GetData");

                var result2 = CacheManager.Execute(() =>
                {
                    return Demo1DataLayer.GetWithParameter(p1, p2);
                }, "GetWithParameter" + string.Format("-{0}-{1}", p1, p2));

                Console.WriteLine("GetData: " + result);
                Console.WriteLine("GetWithParameter: " + result2);

                Console.ReadKey();
            }
        }
    }
}
