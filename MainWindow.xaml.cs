using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DopravniPodnikSem.Services;
using System.Windows.Media.Animation;
using DopravniPodnikSem.Views;
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

        private readonly DatabaseService _databaseService;

        public MainWindow()
        {
            InitializeComponent();

            // Настройка конфигурации для DatabaseService (по необходимости)
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _databaseService = new DatabaseService(configuration);

            // Установка DataContext с экземпляром NavigationVM
            DataContext = new NavigationVM(_databaseService);

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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
