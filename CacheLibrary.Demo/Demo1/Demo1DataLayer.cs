using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLibrary.Demo
{
    public class Demo1DataLayer
    {
        [AutomaticCache("GetData", 1)] /*Cache for 1m*/
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
