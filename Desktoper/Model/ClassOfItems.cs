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

namespace Desktoper.Model
{
    [Serializable]
    class ClassOfItems
    {
        public ObservableCollection<Program> ListOfPrograms { get; set; } = new ObservableCollection<Program>();
        public ObservableCollection<Site> ListOfSites { get; set; } = new ObservableCollection<Site>();
        public ObservableCollection<UFile> ListOfFiles { get; set; } = new ObservableCollection<UFile>();

        private static string FileName = "Data.desk";

        private static ClassOfItems instance;
        private static ClassOfItems _instance = new ClassOfItems();
        public static ClassOfItems Instance { get { return _instance; } }

        public static ClassOfItems getInstance()
        {
            if (instance == null)
                instance = new ClassOfItems();
            return instance;
        }

        public ClassOfItems()
        {
            Desearialize();
            ListOfPrograms.CollectionChanged += List_CollectionChanged;
            ListOfSites.CollectionChanged += List_CollectionChanged;
            ListOfFiles.CollectionChanged += List_CollectionChanged;
        }

        private void List_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Save();
        }

        private Task Save()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(stream, instance);
                }
            }
            catch (IOException) { /*if already saving - don`t save */ }

            return Task.FromResult(0);
        }

        public void Desearialize()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                if (stream.Length != 0)
                {
                    GetData(
                        (ClassOfItems)formatter.Deserialize(stream));
                }
            }
        }

        private void GetData(ClassOfItems Inst)
        {
            foreach (UFile file in Inst.ListOfFiles)
            {
                ListOfFiles.Add(file);
            }
            foreach (Site site in Inst.ListOfSites)
            {
                ListOfSites.Add(site);
            }
            foreach (Program prog in Inst.ListOfPrograms)
            {
                ListOfPrograms.Add(prog);
            }
        }
    }
}
