using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Authentication
    {

        public static string getUserName(string windowsIndetity)
        {
            return windowsIndetity.Split('\\')[1];
           
        }
    }
}
