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
        private static string SettingsNameFile = "Sett.dat";

        public static string GetItemsFilePath()
        {
            return FolderPath + "\\" + ItemsNameFile;
        }

        public static string GetSettingsFilePath()
        {
            return FolderPath + "\\" + ItemsNameFile;
        }
    }
}
