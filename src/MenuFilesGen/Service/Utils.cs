using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuFilesGen.Service
{
    public class Utils
    {
        public static int  StringToInt(string str, int def=0)
        {
            int result;

            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                return def;
            }
             
        }
    }
}
