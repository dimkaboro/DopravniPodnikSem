using DopravniPodnikSem.ViewModels;
using System.Windows;
using DopravniPodnikSem.ViewModels;

namespace DopravniPodnikSem.Views
{
    public partial class RegistrationView : Window
    {
        public RegistrationView()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
        }
    }
}
