using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;
using DopravniPodnikSem;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

public class NavigationVM : INotifyPropertyChanged
{
    private Role? _userRole;
    private object _currentView;

    // Инициализируем роль как null по умолчанию
    public Role? UserRole
    {
        get => _userRole;
        set
        {
            if (_userRole != value)
            {
                _userRole = value;
                OnPropertyChanged(nameof(UserRole));
                OnPropertyChanged(nameof(IsRoleInfoVisible));
            }
        }
    }

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged(nameof(CurrentView));
        }
    }


    // Свойства для управления видимостью кнопок
    public bool IsRoleInfoVisible => UserRole != null; // Видимость информации о роли, если UserRole != null

    public ICommand CheckDatabaseConnectionCommand { get; }
    private readonly DatabaseService _databaseService;

    public NavigationVM(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        CheckDatabaseConnectionCommand = new ViewModelCommand(ExecuteCheckDatabaseConnection);
        CurrentView = new HomeView();
    }

    // Метод для авторизации
    public void Authorized(Zamestnanec zamestnanec)
    {
        UserRole = zamestnanec.Role; // Устанавливаем роль пользователя
        
        if (UserRole == Role.Administrator)
        {
            var adminWindow = new AdminWindow();
            adminWindow.Show();
        }
        else if (UserRole == Role.Zamestnanec)
        {
            var employeeWindow = new EmployeeWindow();
            employeeWindow.Show();
        }

        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow)
            {
                window.Close();
                break;
            }
        }
    }

    public void Registered()
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow mainWindow)
            {
                CurrentView = new LoginView();
                break;
            }
        }
    }

    private async void ExecuteCheckDatabaseConnection(object parameter)
    {
        bool isConnected = await _databaseService.TestConnectionAsync();
        MessageBox.Show(isConnected ? "Success!" : "Failed!");
    }

    // Обработчик изменений для INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
