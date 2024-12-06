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

            // Настройка конфигурации для DatabaseService
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            // Установка DataContext с экземпляром NavigationVM
            _navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            DataContext = _navigationVM;  // Устанавливаем DataContext на NavigationVM
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
            // Устанавливаем основное содержимое в MainContent на главный экран
            _navigationVM.CurrentView = new HomeView();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем содержимое на LoginView
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
