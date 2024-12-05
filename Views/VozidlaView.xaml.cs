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

namespace DopravniPodnikSem.Views
{
    /// <summary>
    /// Interaction logic for VozidlaView.xaml
    /// </summary>
    public partial class VozidlaView : UserControl
    {
        public VozidlaView()
        {
            InitializeComponent();
        }

        private void VehiclesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
