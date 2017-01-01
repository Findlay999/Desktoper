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
using System.Windows.Shapes;

namespace Desktoper
{
    public partial class DesktoperDialog : Window
    {
        public bool IsDialog = true;
         
        public DesktoperDialog()
        {
            InitializeComponent();
        }

        private void OkB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void CancB_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(!IsDialog)
            {
                OkB.Visibility = Visibility.Collapsed;
                CancB.Visibility = Visibility.Collapsed;
                CloseB.Visibility = Visibility.Visible;
            }
            else
            {
                OkB.Visibility = Visibility.Visible;
                CancB.Visibility = Visibility.Visible;
                CloseB.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseB_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
