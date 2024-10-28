using DopravniPodnikSem;
using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DopravniPodnikSem.Views;

namespace DopravniPodnikSem.ViewModels
{
    public class NavigationVM
    {
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
}
}
