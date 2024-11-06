using DopravniPodnikSem;
using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DopravniPodnikSem.Views;
using DopravniPodnikSem.Services;

namespace DopravniPodnikSem.ViewModels
{
    public class NavigationVM
    {
        // Команда для проверки подключения к базе данных
        public ICommand CheckDatabaseConnectionCommand { get; }

        private readonly DatabaseService _databaseService;

        public NavigationVM(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            // Инициализация команды
            CheckDatabaseConnectionCommand = new ViewModelCommand(ExecuteCheckDatabaseConnection);
        }

        // Метод для навигации после успешной авторизации
        public void Authorized(UserData user)
        {
            // Открытие главного окна после успешной авторизации
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // Закрытие текущего окна логина
            CloseLoginWindow();
        }

        // Метод для закрытия окна логина
        private void CloseLoginWindow()
        {
            // Перебираем все открытые окна и закрываем окно логина
            foreach (Window window in Application.Current.Windows)
            {
                if (window is LoginView)
                {
                    window.Close();
                    break;
                }
            }
        }

        // Метод для выполнения команды проверки подключения к базе данных
        private async void ExecuteCheckDatabaseConnection(object parameter)
        {
            bool isConnected = await _databaseService.TestConnectionAsync();
            MessageBox.Show(isConnected ? "Подключение успешно!" : "Ошибка подключения!");
        }
    }
}
