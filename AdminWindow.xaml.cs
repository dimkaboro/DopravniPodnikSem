using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Microsoft.Extensions.Configuration;
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
        private readonly PasswordService _passwordService;

        public AdminWindow()
        {
            InitializeComponent();

            // Настройка конфигурации для DatabaseService
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var databaseService = new DatabaseService(configuration);

            _userDataRepository = new UserDataRepository(databaseService, _passwordService);

            // Установка DataContext с экземпляром NavigationVM
            _navigationVM = new NavigationVM(databaseService);
            DataContext = _navigationVM;  // Устанавливаем DataContext на NavigationVM

            // Сразу устанавливаем роль как null
            _navigationVM.UserRole = null;
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

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileView = new ProfileView
            {
                DataContext = new ProfileViewModel(_userDataRepository, CurrentSession.LoggedInUser.ZamestnanecId, CurrentSession.LoggedInUser.AdresaId)
            };
            MainContent.Content = profileView;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем основное содержимое в MainContent на главный экран
            MainContent.Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Width = 400,
                Children =
                {
                    new Image
                    {
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Image/Logo2.png")),
                        Width = 100,
                        Height = 80,
                        Margin = new Thickness(0, 0, 0, 10),
                        Opacity = 0.8
                    },
                    new TextBlock
                    {
                        Text = "Dopravni Podnik",
                        FontSize = 30,
                        FontWeight = FontWeights.Bold,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d3033")),
                        TextAlignment = TextAlignment.Center,
                        FontFamily = new FontFamily("Gill Sans")
                    }
                }
            };
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
