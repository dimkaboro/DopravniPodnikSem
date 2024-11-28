using System.Windows.Controls;
using DopravniPodnikSem.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnikSem.Views
{
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetService<RegistrationViewModel>();
        }
    }
}
