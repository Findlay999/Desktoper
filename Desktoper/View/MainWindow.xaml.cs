using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktoper.ViewModel;
using Desktoper.Model;

namespace Desktoper.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new SettingsViewModel();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Visible;
            prog.Visibility = Visibility.Hidden;
            Sett.Visibility = Visibility.Hidden;
            Cons.Visibility = Visibility.Hidden;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            prog.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Hidden;
            Cons.Visibility = Visibility.Hidden;
            Sett.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Cons.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Hidden;
            prog.Visibility = Visibility.Hidden;
            Sett.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (this.ActualWidth != 400 && this.ActualHeight != 30)
            {
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
                this.Width = 400;
                this.Height = 30;
                this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 20;
                this.Top = 20;
                Cons.Visibility = Visibility.Visible;
                add.Visibility = Visibility.Hidden;
                prog.Visibility = Visibility.Hidden;
                Sett.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Width = 600;
                this.Height = 350;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.ResizeMode = ResizeMode.CanResize;
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2; 
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Cons.Visibility = Visibility.Hidden;
            add.Visibility = Visibility.Hidden;
            prog.Visibility = Visibility.Hidden;
            Sett.Visibility = Visibility.Visible;
        }
    }
}
