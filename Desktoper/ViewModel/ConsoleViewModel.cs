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
    public struct CommandInfo
    {
        public string CommandName { get; set; }
        public string CommandDesc { get; set; }

        public CommandInfo(string Name, string Desc)
        {
            CommandName = Name;
            CommandDesc = Desc;
        }
    }

    class ConsoleViewModel : INotifyPropertyChanged
    {
        #region Variables
        public ClassOfItems Items { get; set; } = ClassOfItems.getInstance();

        public ObservableCollection<CommandInfo> CommandsInfo { get; set; } = new ObservableCollection<CommandInfo>();

        private bool AllowSearchMatches = true;
        
        private string[] ProgramCommands = { "OpenProgram" };
        private string[] FileCommands = { "OpenFile" };
        private string[] SiteCommands = { "OpenSite" };

        private string console_command;

        private enum Keys  { Program, File, Site }       
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

        public ObservableCollection<Object> searchMatches;
        #endregion

        #region GetSet
        public ObservableCollection<Object> SearchMatches
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
        #endregion

        #region Commands Variables
        private ICommand fastOpen_command;
        private ICommand scroll_resultDown;
        private ICommand scroll_resultUp;
        private ICommand handlecommand;
        #endregion

        #region Commands GetSet 
        public ICommand FastOpenCommand
        {
            get { return fastOpen_command; }
            set
            {
                fastOpen_command = value;
                OnPropertyChanged("FastOpenCommand");
            }
        }

        public ICommand ScrollResultDown
        {
            get { return scroll_resultDown; }
            set
            {
                scroll_resultDown = value;
                OnPropertyChanged("ScrollResultDown");
            }
        }

        public ICommand ScrollResultUp
        {
            get { return scroll_resultUp; }
            set
            {
                scroll_resultUp = value;
                OnPropertyChanged("ScrollResultUp");
            }

        }

        public ICommand HandleCommand
        {
            get { return handlecommand; }
            set
            {
                handlecommand = value;
                OnPropertyChanged("HandleCommand");
            }
        }
        #endregion

        public ConsoleViewModel()
        {
            FillCommandsInfo();
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
                RewriteCommandByItem(LocalCommand);
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
                RewriteCommandByItem(LocalCommand);
                AllowSearchMatches = true;
            }
        }

        private void RewriteCommandByItem(Tuple<string,string,Keys> LocalCommand)
        {
            if (SearchMatches[SelIndex] is Program)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (SearchMatches[SelIndex] as Program).Name;
            }
            if (SearchMatches[SelIndex] is Site)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (SearchMatches[SelIndex] as Site).Name;
            }
            if (SearchMatches[SelIndex] is UFile)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (SearchMatches[SelIndex] as UFile).Name;
            }
        }

        private void FindMatches()
        {
            if (AllowSearchMatches)
            {
                SelIndex = -1;
                SearchMatches = new ObservableCollection<Object>();
                if (String.IsNullOrEmpty(ConsoleCommand))
                {
                    return;
                }
                var LocalCommand = CheckCommand();

                if(LocalCommand.Item3 == Keys.Program)
                foreach (Program prog in Items.ListOfPrograms)
                {
                    if (CompareWithoutCaseAndSpace(prog.Name, LocalCommand.Item2))
                    {
                        SearchMatches.Add(prog);
                    }
                }

                if (LocalCommand.Item3 == Keys.Site)
                foreach (Site prog in Items.ListOfSites)
                {
                    if (CompareWithoutCaseAndSpace(prog.Name, LocalCommand.Item2))
                    {
                        SearchMatches.Add(prog);
                    }
                }

                if (LocalCommand.Item3 == Keys.File)

                foreach (UFile prog in Items.ListOfFiles)
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

            if (LocalCommand.Item3 == Keys.Program)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (obj as Program).Name;
            }
            if(LocalCommand.Item3 == Keys.File)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (obj as UFile).Name;
            }
            if(LocalCommand.Item3 == Keys.Site)
            {
                ConsoleCommand = LocalCommand.Item1 + " " + (obj as Site).Name;
            }

            SearchMatches = new ObservableCollection<Object>();
            DoCommand(null);
        }

        private void DoCommand(object obj)
        {
            var LocalCommand = CheckCommand();
            try
            {
                if (LocalCommand.Item1 == "OpenProgram")
                {
                    var command = ConsoleCommand.Remove(0, "OpenProgram".Length + 1);
                    System.Diagnostics.Process.Start(
                        Items.ListOfPrograms.First(x => String.Compare(x.Name, command, true) == 0).WorkingDirectory);
                }
                
                if(LocalCommand.Item1 == "OpenSite")
                {
                    var command = ConsoleCommand.Remove(0, "OpenSite".Length + 1);
                    System.Diagnostics.Process.Start(
                        Items.ListOfSites.First(x => String.Compare(x.Name, command, true) == 0).URL); 
                }

                if (LocalCommand.Item1 == "OpenFile")
                {
                    var command = ConsoleCommand.Remove(0, "OpenFile".Length + 1);
                    System.Diagnostics.Process.Start(
                        Items.ListOfFiles.First(x => String.Compare(x.Name, command, true) == 0).FilePath);
                }
            }
            catch
            {
                MessageBox.Show("Cannot open this file!");
            }
            ConsoleCommand = null;
        }

        private Tuple<string, string, Keys> CheckCommand()
        {
            var Result = FindCommand(ProgramCommands);
            if (Result != null)
            {
                return new Tuple<string, string, Keys>(Result.Item1, Result.Item2, Keys.Program);
            }
            else
            {
                Result = FindCommand(SiteCommands);
                if (Result != null)
                {
                    return new Tuple< string, string, Keys> (Result.Item1, Result.Item2, Keys.Site);
                }
                else
                {
                    Result = FindCommand(FileCommands);
                    if(Result != null)
                    {
                        return new Tuple<string, string, Keys>(Result.Item1, Result.Item2, Keys.File);
                    }
                }
            }

            return null;
        }

        private Tuple<string, string> FindCommand(string[] CommandsList)
        {
            foreach (string command in CommandsList)
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

        private void FillCommandsInfo()
        {
            CommandsInfo.Add(
                new CommandInfo("OpenFile", "Открыть файл"));
            CommandsInfo.Add(
                new CommandInfo("OpenSite", "Открыть ccылку"));
            CommandsInfo.Add(
                new CommandInfo("OpenProgram", "Открыть программу"));
        }
        #region PropertyChanged
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
