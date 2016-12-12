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
using System.Globalization;
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
    class ConsoleViewModel : INotifyPropertyChanged
    {
        public ProgramsViewModel AllPrograms { get; set; } = ProgramsViewModel.getInstance();

        private bool AllowSearchMatches = true;
        
        private string[] AllCommands = { "open", "remove", "add", "GoTo" };

        private int sel_index = -1;

        public int SelIndex
        {
            get { return sel_index; }
            set
            {
                sel_index = value;
                OnPropertyChanged("SelIndex");
            }
        }

        public ObservableCollection<Program> searchMatches;

        public ObservableCollection<Program> SearchMatches
        {
            get
            {
                return searchMatches;
            }
            set
            {
                searchMatches = value;
                OnPropertyChanged("SearchMatches");
            }
        }

        private ICommand fastOpen_command;

        public ICommand FastOpenCommand
        {
            get { return fastOpen_command; }
            set
            {
                fastOpen_command = value;
                OnPropertyChanged("FastOpenCommand");
            }
        }

        private ICommand scroll_resultDown;

        public ICommand ScrollResultDown
        {
            get { return scroll_resultDown; }
            set
            {
                scroll_resultDown = value;
                OnPropertyChanged("ScrollResultDown");
            }
        }

        private ICommand scroll_resultUp;

        public ICommand ScrollResultUp
        {
            get { return scroll_resultUp; }
            set
            {
                scroll_resultUp = value;
                OnPropertyChanged("ScrollResultUp");
            }

        }

        private ICommand handlecommand;

        public ICommand HandleCommand
        {
            get { return handlecommand; }
            set
            {
                handlecommand = value;
                OnPropertyChanged("HandleCommand");
            }
        }
        
        private string console_command;

        public string ConsoleCommand
        {
            get { return console_command; }
            set
            {                
                console_command = value;
                OnPropertyChanged("ConsoleCommand");
                FindMatches();
            }
        }

        public ConsoleViewModel()
        {
            HandleCommand = new RelayCommand(DoCommand);
            FastOpenCommand = new RelayCommand(FastCommand);
            ScrollResultDown = new RelayCommand(ScrollDown);
            ScrollResultUp = new RelayCommand(ScrollUp);
        }

        private void ScrollDown(object obj)
        {
            if (SelIndex < SearchMatches.Count - 1)
            {
                SelIndex++;
                var LocalCommand = CheckCommand();
                AllowSearchMatches = false;                
                ConsoleCommand = LocalCommand.Item1 + " " + SearchMatches[SelIndex].Name;
                AllowSearchMatches = true;
            }
        }

        private void ScrollUp(object obj)
        {
            if (SelIndex > 0)
            {
                SelIndex--;
                var LocalCommand = CheckCommand();
                AllowSearchMatches = false;
                ConsoleCommand = LocalCommand.Item1 + " " + SearchMatches[SelIndex].Name;
                AllowSearchMatches = true;
            }
        }

        private void FindMatches()
        {
            if (AllowSearchMatches)
            {
                SelIndex = -1;
                SearchMatches = new ObservableCollection<Program>();
                if (String.IsNullOrEmpty(ConsoleCommand))
                {
                    return;
                }
                var LocalCommand = CheckCommand();

                foreach (Program prog in AllPrograms.ListOfPrograms)
                {
                    if (CompareWithoutCaseAndSpace(prog.Name, LocalCommand.Item2))
                    {
                        SearchMatches.Add(prog);
                    }
                }
            }
        }

        private void FastCommand(object obj)
        {
            var LocalCommand = CheckCommand();
            ConsoleCommand = LocalCommand.Item1 + " " + (obj as Program).Name;
            SearchMatches = new ObservableCollection<Program>();
            DoCommand(null);
        }

        private void DoCommand(object obj)
        {
            var LocalCommand = CheckCommand();
            try
            {
                if (LocalCommand.Item1 == "Open" || LocalCommand.Item1 == "open")
                {
                    var command = ConsoleCommand.Remove(0, 5);
                    System.Diagnostics.Process.Start(
                        AllPrograms.ListOfPrograms.First(x => String.Compare(x.Name, command, true) == 0).WorkingDirectory);
                }
                
                if(LocalCommand.Item1 == "GoTo" || LocalCommand.Item1 == "Goto")
                {
                    var command = ConsoleCommand.Remove(0, 5);
                    System.Diagnostics.Process.Start("http://" +
                        AllPrograms.ListOfSites.First(x => String.Compare(x.Name, command, true) == 0).URL); 
                }
            }
            catch
            {
                MessageBox.Show("Cannot open this file!");
            }
            ConsoleCommand = null;
        }

        private Tuple<string, string> CheckCommand()
        {
            foreach (string command in AllCommands)
            {
                if (ConsoleCommand.Length >= command.Length && ConsoleCommand.Substring(0, command.Length).ToLower() == command.ToLower())
                {
                    return new Tuple<string, string>(ConsoleCommand.Substring(0, command.Length), ConsoleCommand.Remove(0, command.Length));
                }
            }
            return null;
        }

        private bool CompareWithoutCaseAndSpace(string Text, string Checker)
        {
            try
            {
                Text = Text.Replace(" ", null);
                Checker = Checker.Replace(" ", null);
                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(Text, Checker, CompareOptions.IgnoreCase) >= 0)
                {
                    return true;
                }
                return false;
            }
            catch { return false; };    
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
