using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 书店管理系统
{
    class common
    {
        private static string username = "";
        public static string GetName()
        {
            return username; 
        }
        public static void SetName(string value)
        {
            username = value;
        }
    }
}
