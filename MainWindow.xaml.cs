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
using System.Windows;
using System.Windows.Media.Animation;

namespace DopravniPodnikSem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isMenuOpen = false; // Переменная для отслеживания состояния меню

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BurgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMenuOpen)
            {
                // Закрываем меню
                MenuColumn.Width = new GridLength(0);
                SideMenu.Visibility = Visibility.Collapsed; // Скрываем меню
                _isMenuOpen = false;
            }
            else
            {
                // Открываем меню
                SideMenu.Visibility = Visibility.Visible; // Показываем меню
                MenuColumn.Width = new GridLength(228);
                _isMenuOpen = true;
            }
        }

        private void OpenMenu()
        {
            SideMenu.Visibility = Visibility.Visible; // Показываем меню

            // Запускаем анимацию открытия
            var openAnimation = (Storyboard)FindResource("OpenMenuAnimation");
            openAnimation.Completed += (s, e) =>
            {
                MenuColumn.Width = new GridLength(200); // Устанавливаем ширину колонки после завершения анимации
            };
            openAnimation.Begin(); // Запускаем анимацию открытия
            _isMenuOpen = true; // Обновляем состояние
        }

        private void CloseMenu()
        {
            // Запускаем анимацию закрытия
            var closeAnimation = (Storyboard)FindResource("CloseMenuAnimation");
            closeAnimation.Completed += (s, e) =>
            {
                SideMenu.Visibility = Visibility.Collapsed; // Скрываем меню после завершения анимации
                MenuColumn.Width = new GridLength(0); // Устанавливаем ширину колонки после завершения анимации
            };
            closeAnimation.Begin(); // Запускаем анимацию закрытия
            _isMenuOpen = false; // Обновляем состояние
        }

        // Обработчики событий для кнопок Login и Registration
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для входа
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для регистрации
        }
    }
}
