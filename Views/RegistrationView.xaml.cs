using System.Windows.Controls;
using DopravniPodnikSem.ViewModels;

namespace DopravniPodnikSem.Views
{
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
            DataContext = new RegistrationViewModel();
        }
    }
}
