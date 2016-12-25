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
    class AddingElementsViewModel : INotifyPropertyChanged
    {
        #region Site variables
        public string SiteName { get; set; }
        public string SiteURL  { get; set; }
        public string SiteDesc { get; set; }
        #endregion

        #region Commands
        private ICommand addSite;

        public ICommand AddSite
        {
            get { return addSite; }
            set
            {
                addSite = value;
                OnPropertyChanged("AddSite");
            }
        }

        private ICommand changeVisiblePanel;

        public ICommand ChangeVisiblePanel
        {
            get { return changeVisiblePanel; }
            set
            {
                changeVisiblePanel = value;
                OnPropertyChanged("ChangeVisiblePanel");
            }
        }
        #endregion

        private string visiblePanelKey;
        public string VisiblePanelKey
        {
            get { return visiblePanelKey; }
            set
            {
                visiblePanelKey = value;
                OnPropertyChanged("VisiblePanelKey");
            }
        }

        private ClassOfItems Items { get; set; } = ClassOfItems.getInstance();
        public string[] Types { get; set; } = new string[] { "File", "Label", "Site" };

        public AddingElementsViewModel()
        {
            ChangeVisiblePanel = new RelayCommand(ChangePanel);
            AddSite = new RelayCommand(AddSiteToList);
        }

        #region Command methods
        private void AddSiteToList(object obj)
        {
            Site site = (new Site()
            {
                Name = SiteName,
                URL = SiteURL,
                Desc = SiteDesc
            });
            if (!Items.ListOfSites.Any(x => x.URL == site.URL || x.Name == site.Name))
            {
                Items.ListOfSites.Add(site);
            }
        }

        private void ChangePanel(object obj)
        {
            foreach(string type in Types)
            {
                if(type == (obj as string))
                {
                    VisiblePanelKey = type;
                    return;
                }
            }
            VisiblePanelKey = null;
        }
        #endregion

        #region Other methods
        public void GetDataPrograms(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] ForAdd = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (VisiblePanelKey == "Label")
                {
                    AddPrograms(ForAdd);
                }
                if (VisiblePanelKey == "File")
                {
                    AddFiles(ForAdd);
                }
            }
        }

        private void AddFiles(string[] FilesForAdd)
        {
            foreach(string File in FilesForAdd)
            {
                FileInfo info = new FileInfo(File);
                UFile file = new UFile()
                {
                    Name = info.Name.Replace(info.Extension, null),
                    FilePath = info.FullName,
                    ImgPath = info.FullName
                };
                if(!Items.ListOfFiles.Any(x => x.FilePath == file.FilePath))
                {
                    Items.ListOfFiles.Add(file);
                }
            }
        }

        private void AddPrograms(string[] ProgramsForAdd)
        {
            foreach (string Prog in ProgramsForAdd)
            {
                FileInfo info = new FileInfo(Prog);
                WshShell shell = new WshShell();
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(Prog);

                Program prog = new Program()
                {
                    Name = info.Name.Replace(info.Extension, null),
                    WorkingDirectory = link.TargetPath,
                    ImgPath = link.TargetPath
                };

                if (!Items.ListOfPrograms.Any(x => x.WorkingDirectory == prog.WorkingDirectory))
                    Items.ListOfPrograms.Add(prog);
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
