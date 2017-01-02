using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Desktoper.Model;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;
using IWshRuntimeLibrary;
using Desktoper.Commands;
using Desktoper.Other.CustomDialog;

namespace Desktoper.ViewModel
{
    class AddingElementsViewModel : INotifyPropertyChanged
    {
        #region Site data variables
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

        #region Other variables
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
        #endregion

        #region Constructors
        public AddingElementsViewModel()
        {
            ChangeVisiblePanel = new RelayCommand(ChangePanel);
            AddSite = new RelayCommand(AddSiteToList);
        }
        #endregion

        #region Command methods
        private void AddSiteToList(object obj)
        {
            
            string imgPath = null;
            try
            {
                using (WebClient client = new WebClient())
                {
                    //попытка загрузка иконки сайта
                    client.DownloadFile("https://www.google.com/s2/favicons?domain_url=" + SiteURL, SaveData.FolderPath + SiteName + ".siteIco");
                    imgPath = SaveData.FolderPath + SiteName + ".siteIco";
                }
            }
            catch
            {
                DialogWindow.Show("Ошибка добавления ссылки...");
            }

            Site site = (new Site()
            {
                Name = SiteName,
                URL = SiteURL,
                Desc = SiteDesc,
                ImgPath = imgPath
            });

            //проверка нет ли такого сайта в списке
            if (site.Name != null && !Items.ListOfSites.Any(x => x.URL == site.URL || x.Name == site.Name))
            {
                Items.ListOfSites.Add(site);
            }
            else
            {
                DialogWindow.Show("Не удалось добавить сcылку. Возможно, такая ссылка уже была добавлена!");
            }
        }

        private void ChangePanel(object obj)
        {
            foreach(string type in Types)
            {
                if(type == (obj as string))
                {
                    VisiblePanelKey = type; // получаем тип окна которое нужно отобразить
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
                string[] ForAdd = (string[])e.Data.GetData(DataFormats.FileDrop); // берется информация о файле
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

                //проверка наличия такого файла
                if(!Items.ListOfFiles.Any(x => x.FilePath == file.FilePath))
                {
                    Items.ListOfFiles.Add(file);
                }
                else
                {
                    DialogWindow.Show("Не удалось добавить файл. Возможно, такой файл уже был добавлен!");
                }
            }
        }

        private void AddPrograms(string[] ProgramsForAdd)
        {
            foreach (string Prog in ProgramsForAdd)
            {
                FileInfo info = new FileInfo(Prog);
                WshShell shell = new WshShell(); // собираются данные о ярлыке
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(Prog);

                Program prog = new Program()
                {
                    Name = info.Name.Replace(info.Extension, null),
                    WorkingDirectory = link.TargetPath,
                    ImgPath = link.TargetPath
                };

                //проверка наличия ссылки на такую же программу
                if (!Items.ListOfPrograms.Any(x => x.WorkingDirectory == prog.WorkingDirectory))
                    Items.ListOfPrograms.Add(prog);
                else
                {
                    DialogWindow.Show("Не удалось добавить ярлык. Возможно, такой ярлык уже был добавлен!");
                }
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
