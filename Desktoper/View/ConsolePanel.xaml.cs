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
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Desktoper.ViewModel;

namespace Desktoper.View
{
    public partial class ConsolePanel : UserControl
    {
        public ConsolePanel()
        {
            this.DataContext = new ConsoleViewModel();
            InitializeComponent();           
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Window w = Window.GetWindow(this);

            if (null != w)
            {
                w.LocationChanged += delegate (object sender2, EventArgs args)
                {
                    var offset = popUP.HorizontalOffset;
                    popUP.HorizontalOffset = offset + 1;
                    popUP.HorizontalOffset = offset;
                };

                w.SizeChanged += delegate (object sender3, SizeChangedEventArgs e2)
                {
                    var offset = popUP.HorizontalOffset;
                    popUP.HorizontalOffset = offset + 1;
                    popUP.HorizontalOffset = offset;
                };
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List.ScrollIntoView(List.SelectedItem);
        }
    }
}
