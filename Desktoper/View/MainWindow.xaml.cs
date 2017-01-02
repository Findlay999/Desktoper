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
using Desktoper.Other.CustomDialog;

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
            this.DataContext = new MainWindowViewModel();
            DialogWindow.Owner = this;      
        }

        private void MinimizeClick(object sender, RoutedEventArgs e)
        {
            int minWidth = 400;
            int minHeight = 30;
            int minTop = 20;

            int maxWidth = 720;
            int maxHeight = 380;

            if (this.ActualWidth != minWidth && this.ActualHeight != minHeight)
            {
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
                this.Width = minWidth;
                this.Height = minHeight;
                this.Left = SystemParameters.PrimaryScreenWidth - this.Width - minTop;
                this.Top = minTop;
                MenuBord.Margin = new Thickness(0);
                MinimizeButt.BorderThickness = new Thickness(0);
                MinimizeButt.Content = "Развернуть";
                MinimizeButt.Visibility = Visibility.Visible;
                (this.DataContext as MainWindowViewModel).DoChange(2);
            }
            else
            {
                this.Width = maxWidth;
                this.Height = maxHeight;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.ResizeMode = ResizeMode.CanResize;
                this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
                this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;
                MinimizeButt.BorderThickness = new Thickness(1);
                MinimizeButt.Content = "Cвернуть";
            }
        }
    }
}
