using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Desktoper.Commands;
using System.Threading.Tasks;

namespace Desktoper.ViewModel
{

    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Variables
        public SettingsViewModel DesktoperSettings { get; set; } = new SettingsViewModel();

        public ObservableCollection<Visibility> VisibilityInfo { get; set; } = new ObservableCollection<Visibility>();
        #endregion

        #region Command variables
        private ICommand changeVisibility;

        public ICommand ChangeVisibility
        {
            get { return changeVisibility; }
            set
            {
                changeVisibility = value;
                OnPropertyChanged("ChangeVisibility");
            }
        }
        #endregion

        #region Constructors
        public MainWindowViewModel()
        {
            //заполнение массива даных о видимости панелей
            VisibilityInfo.Add(Visibility.Hidden);
            VisibilityInfo.Add(Visibility.Visible);
            VisibilityInfo.Add(Visibility.Hidden);
            VisibilityInfo.Add(Visibility.Hidden);

            ChangeVisibility = new RelayCommand(DoChange);
        }
        #endregion

        #region Command methods
        public void DoChange(object obj) // отображание панели по заданому индексу
        {
            for (int i = 0; i < VisibilityInfo.Count; i++)
                VisibilityInfo[i] = Visibility.Hidden;
            int index = Convert.ToInt16(obj);
            VisibilityInfo[index] = Visibility.Visible;
        }
        #endregion

        #region PropertyChanged
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
