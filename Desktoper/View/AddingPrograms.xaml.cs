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

namespace Desktoper.View
{
    /// <summary>
    /// Interaction logic for AddingPrograms.xaml
    /// </summary>
    public partial class AddingPrograms : UserControl
    {
        public AddingPrograms()
        {
            this.DataContext = new AddingElementsViewModel();
            InitializeComponent();
        }

        private void ProgPanel_Drop(object sender, DragEventArgs e)
        {
            ((AddingElementsViewModel)this.DataContext).GetDataPrograms(e);
        }
    }
}
