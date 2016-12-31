using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktoper.Model
{
    [Serializable]
    public class Settings
    {
        public static bool isStartUp;
        public static bool isOnTop;

        static Settings()
        {
            isStartUp = false;
            isOnTop = false;
        }
    }
}
