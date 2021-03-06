﻿using System;
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
using Desktoper.Other.CustomDialog;

namespace Desktoper.ViewModel
{
    class ProgramsViewModel : INotifyPropertyChanged
    {
        #region Variables

        public ClassOfItems ListOfItems { get; set; } = ClassOfItems.getInstance();

        private enum Keys { Program, File, Site }

        private Program selectedProgram = null;
        private Site selectedSite = null;
        private UFile selectedFile = null;

        #endregion

        #region Command variables
        private ICommand deleteElement;
        private ICommand openElement;
        private ICommand hideElement;
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
        #endregion

        #region Command Getters adn Setters 
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

        public ICommand HideElement
        {
            get { return hideElement; }
            set { hideElement = value; }
        }
        #endregion

        #region Constructors
        public ProgramsViewModel() 
        {
            DeleteElement = new RelayCommand(DeleteElm, x => x != null);
            HideElement = new RelayCommand(HideElm);
            OpenElement = new RelayCommand(OpenElm);
        }
        #endregion

        #region Command Methods
        public void HideElm(object obj) // скрываем панель информации путем обнуления информации о выбраном файле
        {
            if (typeof(Program) == obj.GetType())
            {
                SelectedProgram = null;
            }

            if (typeof(Site) == obj.GetType())
            {
                SelectedSite = null;
            }

            if (typeof(UFile) == obj.GetType())
            {
                SelectedFile = null;
            }
        }

        public void DeleteElm(object obj) // удаление выбраного елемента
        {
            if (typeof(Program) == obj.GetType())
            {
                if (DialogWindow.ShowDialog("Delete this program?"))
                {
                    ListOfItems.ListOfPrograms.Remove(SelectedProgram);
                }
            }

            if (typeof(Site) == obj.GetType())
            {
                if (DialogWindow.ShowDialog("Delete this site?"))
                {
                    ListOfItems.ListOfSites.Remove(SelectedSite);
                }
            }

            if (typeof(UFile) == obj.GetType())
            {
                if (DialogWindow.ShowDialog("Delete this file?"))
                {
                    ListOfItems.ListOfFiles.Remove(SelectedFile);
                }
            }
        }

        public void OpenElm(object obj) // открытие вибраного елемента
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
                DialogWindow.Show("Cannot open this!");
            }
        }
        #endregion

        #region PropertiesChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
