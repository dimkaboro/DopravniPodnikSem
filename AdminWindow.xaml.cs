using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;
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
using System.Windows.Shapes;

namespace DopravniPodnikSem
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private bool _isMenuOpen = false;
        private readonly NavigationVM _navigationVM;
        private readonly IUserDataRepository _userDataRepository;
        private readonly IAdresyRepository _adresyRepository;
        private readonly ISouboryRepository _souboryRepository;

        public AdminWindow()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            _userDataRepository = App.ServiceProvider.GetService<IUserDataRepository>();
            _adresyRepository = App.ServiceProvider.GetService<IAdresyRepository>();
            _souboryRepository = App.ServiceProvider.GetService<ISouboryRepository>();

            _navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            DataContext = _navigationVM;  
        }

        private void BurgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMenuOpen)
            {
                // Закрываем меню
                MenuColumn.Width = new GridLength(0);
                SideMenu.Visibility = Visibility.Collapsed;
                _isMenuOpen = false;
            }
            else
            {
                // Открываем меню
                SideMenu.Visibility = Visibility.Visible;
                MenuColumn.Width = new GridLength(228);
                _isMenuOpen = true;
            }
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new HomeView();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new ProfileView
            {
                DataContext = new ProfileViewModel(
                    _userDataRepository,
                    _adresyRepository,
                    _souboryRepository,
                    CurrentSession.LoggedInUser.ZamestnanecId,
                    CurrentSession.LoggedInUser.AdresaId,
                    CurrentSession.LoggedInUser.SouborId)
            };
        }

        private void ShowUsersButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new ShowUsersView
            {
                DataContext = App.ServiceProvider.GetService<ShowUsersViewModel>()
            };
        }

        private void EmulationButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new UserEmulationView
            {
                DataContext = App.ServiceProvider.GetService<UserEmulationViewModel>()
            };
        }

        private void LogsButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new LogView
            {
                DataContext = App.ServiceProvider.GetService<LogViewModel>()
            };
        }

        private void LinkyButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new LinkyView
            {
                DataContext = App.ServiceProvider.GetService<LinkaViewModel>()
            };
        }

        private void RidiciButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new RidiciView
            {
                DataContext = App.ServiceProvider.GetService<RidiciViewModel>()
            };
        }

        private void VozidlaButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new VozidlaView
            {
                DataContext = App.ServiceProvider.GetService<VozidloViewModel>()
            };
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UdrzbaVozidlaButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new UdrzbaVozidlaView
            {
                DataContext = App.ServiceProvider.GetService<UdrzbaVozidlaViewModel>()
            };
        }

        private void ZastavkyButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new ZastavkyView
            {
                DataContext = App.ServiceProvider.GetService<ZastavkaViewModel>()
            };
        }

        private void JizdyButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new JizdyView
            {
                DataContext = App.ServiceProvider.GetService<JizdaViewModel>()
            };
        }
    }
}
