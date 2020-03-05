using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 书店管理系统
{
    class search_supinfo
    {
        private static string sup_no = "";
        public static string GetName()
        {
            return sup_no;
        }
        public static void SetName(string value)
        {
            sup_no = value;
        }
    }
}
