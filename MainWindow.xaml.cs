using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using Microsoft.Extensions.Configuration;

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
            _navigationVM = new NavigationVM(databaseService);
            DataContext = _navigationVM;  // Устанавливаем DataContext на NavigationVM

            // Сразу устанавливаем роль как null
            _navigationVM.UserRole = null;

            this.Closing += MainWindow_Closing;
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем содержимое на LoginView
            MainContent.Content = new Views.LoginView();
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.RegistrationView();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
