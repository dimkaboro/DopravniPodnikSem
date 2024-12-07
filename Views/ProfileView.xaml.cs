using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
    /// Логика взаимодействия для ProfileView.xaml
    /// </summary>
    public partial class ProfileView : UserControl
    {
        private readonly NavigationVM _navigationVM;

        public ProfileView()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            _navigationVM = App.ServiceProvider.GetService<NavigationVM>();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new EditProfileView
            {
                DataContext = new EditProfileViewModel(
                    App.ServiceProvider.GetService<IUserDataRepository>(),
                    App.ServiceProvider.GetService<IAdresyRepository>(),
                    App.ServiceProvider.GetService<ISouboryRepository>(),
                    ((ProfileViewModel)DataContext).CurrentUser,
                    ((ProfileViewModel)DataContext).CurrentAdresa,
                    ((ProfileViewModel)DataContext).CurrentSoubor)
            };
        }
    }
}
