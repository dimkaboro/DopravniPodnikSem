using DopravniPodnikSem.ViewModels;
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
    /// Interaction logic for SystemKatalogView.xaml
    /// </summary>
    public partial class SystemKatalogView : UserControl
    {
        public SystemKatalogView(SystemKatalogViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // Привязка ViewModel
        }
    }
}
