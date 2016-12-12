using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Desktoper.Model;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using IWshRuntimeLibrary;
using System.Runtime.Serialization.Formatters.Binary;
using Desktoper.Commands;

namespace Desktoper.ViewModel
{
    class ProgramsViewModel : INotifyPropertyChanged
    {
        #region Fields
        private static ProgramsViewModel instance;

        private ICommand deleteElement;
        private ICommand openElement;

        private Program selectedProgram = null;
        private Site selectedSite = null;
        private UFile selectedFile = null;

        public ObservableCollection<Program> ListOfPrograms { get; set; }
        public ObservableCollection<Site> ListOfSites { get; set; }
        public ObservableCollection<UFile> ListOfFiles { get; set; }

       
        private static ProgramsViewModel _instance = new ProgramsViewModel();
        public static ProgramsViewModel Instance { get { return _instance; } }
    
        #endregion

        #region Getters and Setters
        public Program SelectedProgram
        {
            get { return selectedProgram; }
            set
            {
                selectedProgram = value;
                OnPropertyChanged("SelectedProgram");
            }
        }

        public UFile SelectedFile
        {
            get { return selectedFile; }
            set
            {
                selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        public Site SelectedSite
        {
            get { return selectedSite; }
            set
            {
                selectedSite = value;
                OnPropertyChanged("SelectedSite");
            }
        }

        public ICommand DeleteElement
        {
            get { return deleteElement; }
            set { deleteElement = value; }
        }

        public ICommand OpenElement
        {
            get { return openElement; }
            set { openElement = value; }
        }
        #endregion
 
        #region Constructors
        public ProgramsViewModel() 
        {
            ListOfPrograms = new ObservableCollection<Program>();
            ListOfFiles = new ObservableCollection<UFile>();
            ListOfSites = new ObservableCollection<Site>();

            ListOfPrograms.CollectionChanged += ListOfPrograms_CollectionChanged;
            DeleteElement = new RelayCommand(DeleteElm, x => x != null);
            OpenElement = new RelayCommand(OpenElm);

            if (System.IO.File.Exists("List.dat"))
            {
                Desearialize("List.dat");
            }
        }

        public static ProgramsViewModel getInstance()
        {
            if (instance == null)
                instance = new ProgramsViewModel();
            return instance;
        }
        #endregion

        #region Other Methods

       
    
        #endregion

        public void DeleteElm(object obj)
        {
            if (typeof(Program) == obj.GetType())
            {
                if (MessageBox.Show("Delete this program?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ListOfPrograms.Remove(SelectedProgram);
                }
            }

            if (typeof(Site) == obj.GetType())
            {
                if (MessageBox.Show("Delete this site?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ListOfSites.Remove(SelectedSite);
                }
            }

            if (typeof(UFile) == obj.GetType())
            {
                if (MessageBox.Show("Delete this file?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ListOfFiles.Remove(SelectedFile);
                }
            }
        }

        public void OpenElm(object obj)
        {
            try
            {
                if (typeof(Program) == obj.GetType())
                {
                    System.Diagnostics.Process.Start(SelectedProgram.WorkingDirectory);
                }

                if(typeof(Site) == obj.GetType())
                {
                    System.Diagnostics.Process.Start(SelectedSite.URL);
                }

                if (typeof(UFile) == obj.GetType())
                {
                    System.Diagnostics.Process.Start(SelectedFile.FilePath);
                }
            }
            catch
            {
                MessageBox.Show("Cannot open this!");
            }
        }

        public Task SaveData(string FileName)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(stream, ListOfPrograms);
                }
            }
            catch(IOException) { /*if already saving - don`t save */ }

            return Task.FromResult(0);
        }

        public void Desearialize(string FileName)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                if (stream.Length != 0)
                {
                    ObservableCollection<Program> Ds = new ObservableCollection<Program>((ObservableCollection<Program>)formatter.Deserialize(stream));

                    foreach(Program ds in Ds)
                    {
                        ListOfPrograms.Add(ds);
                    }
                }
            }
        }
        #region PropertiesChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private async void ListOfPrograms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await SaveData("List.dat");
        }
        #endregion
    }
}
