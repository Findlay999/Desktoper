using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using Desktoper.Model;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Desktoper.ViewModel
{
    class SettingsViewModel : Settings, INotifyPropertyChanged
    {
        public bool IsOnTop
        {
            get { return isOnTop; }
            set
            {
                isOnTop = value;
                OnPropertyChanged("IsOnTop");
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
            }
        }

        public SettingsViewModel()
        {
            IsStartUp = false;
            IsOnTop = false;
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
                MessageBox.Show("Some error...");
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
