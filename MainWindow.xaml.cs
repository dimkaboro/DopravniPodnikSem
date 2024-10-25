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

namespace DopravniPodnikSem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseTester;

        // Стандартный конструктор без параметров
        public MainWindow()
        {
            InitializeComponent();
        }

        // Ваш существующий конструктор
        public MainWindow(DatabaseService databaseTester) : this()
        {
            _databaseTester = databaseTester;
        }

        private async void TestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            ConnectionStatusTextBlock.Text = "Проверка подключения...";
            bool isConnected = await _databaseTester.TestConnectionAsync();

            if (isConnected)
            {
                ConnectionStatusTextBlock.Text = "Соединение успешно!";
                ConnectionStatusTextBlock.Foreground = System.Windows.Media.Brushes.Green;
            }
            else
            {
                ConnectionStatusTextBlock.Text = "Ошибка подключения.";
                ConnectionStatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
            }
        }
    }
}