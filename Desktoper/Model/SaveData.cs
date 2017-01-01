using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktoper.Model
{
    class SaveData
    {
        private static string FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string ItemsNameFile = "Desk.dat";

        public static string GetItemsFilePath()
        {
            return FolderPath + "\\" + ItemsNameFile;
        }
    }
}
