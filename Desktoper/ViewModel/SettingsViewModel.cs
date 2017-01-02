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
        #region Variables
        public static bool isStartUp;
        public static bool isOnTop;
        #endregion

        #region Getters and Setters
        public bool IsOnTop
        {
            get { return isOnTop; }
            set
            {
                isOnTop = value;
                OnPropertyChanged("IsOnTop");
                Properties.Settings.Default.IsOnTop = value; //обновление настроек
                Properties.Settings.Default.Save(); //сохранение
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
                Properties.Settings.Default.IsStartUp = value; // обновление настроек
                Properties.Settings.Default.Save(); // сохранение
            }
        }
        #endregion

        #region Constructors
        public SettingsViewModel()
        {
            // cчитывание сохраненных данных
            IsStartUp = Properties.Settings.Default.IsStartUp;
            IsOnTop = Properties.Settings.Default.IsOnTop;
        }
        #endregion

        #region Other Methods
        private void SetStartUp() // добавление (удаление) программы в автозагрузку путем внесения записи в регистр
        {         
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    if (IsStartUp)
                    {
                        key.SetValue("Desktoper", System.Reflection.Assembly.GetExecutingAssembly().Location);
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
        #endregion

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
