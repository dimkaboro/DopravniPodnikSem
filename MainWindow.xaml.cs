using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnikSem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isMenuOpen = false;
        private readonly NavigationVM _navigationVM;

        public MainWindow()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            _navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            DataContext = _navigationVM;  
        }

        private void BurgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMenuOpen)
            {
                MenuColumn.Width = new GridLength(0);
                SideMenu.Visibility = Visibility.Collapsed;
                _isMenuOpen = false;
            }
            else
            {
                SideMenu.Visibility = Visibility.Visible;
                MenuColumn.Width = new GridLength(228);
                _isMenuOpen = true;
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new HomeView();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new LoginView();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            _navigationVM.CurrentView = new RegistrationView();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
