using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using Desktoper.Other.CustomDialog;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Desktoper.ViewModel
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        public static bool isStartUp;
        public static bool isOnTop;

        public bool IsOnTop
        {
            get { return isOnTop; }
            set
            {
                isOnTop = value;
                OnPropertyChanged("IsOnTop");
                Properties.Settings.Default.IsOnTop = value;
                Properties.Settings.Default.Save();
            }
        }
        public bool IsStartUp
        {
            get { return isStartUp; }
            set
            {
                isStartUp = value;
                SetStartUp();
                OnPropertyChanged("IsStartUp");
                Properties.Settings.Default.IsStartUp = value;
                Properties.Settings.Default.Save();
            }
        }

        public SettingsViewModel()
        {
            IsStartUp = Properties.Settings.Default.IsStartUp;
            IsOnTop = Properties.Settings.Default.IsOnTop;
        }

        private void SetStartUp()
        {
           
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (IsStartUp)
                    {
                        key.SetValue("Desktoper",System.Reflection.Assembly.GetExecutingAssembly().Location);
                    }
                    else
                    {   
                        key.DeleteValue("Desktoper", false);
                    }
                }
            }
            catch
            {

              DialogWindow.Show("Some error...");
            }
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}   
