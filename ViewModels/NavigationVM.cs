using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;
using DopravniPodnikSem;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows;

public class NavigationVM : INotifyPropertyChanged
{
    private Role? _userRole;

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
                OnPropertyChanged(nameof(IsLoginButtonVisible));
                OnPropertyChanged(nameof(IsRegistrationButtonVisible));
                OnPropertyChanged(nameof(IsRoleInfoVisible));
            }
        }
    }

    // Свойства для управления видимостью кнопок
    public bool IsLoginButtonVisible => UserRole == null; // Кнопка Login видна, если UserRole == null
    public bool IsRegistrationButtonVisible => UserRole == null; // Кнопка Registration видна, если UserRole == null
    public bool IsRoleInfoVisible => UserRole != null; // Видимость информации о роли, если UserRole != null

    public ICommand CheckDatabaseConnectionCommand { get; }
    private readonly DatabaseService _databaseService;

    public NavigationVM(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        CheckDatabaseConnectionCommand = new ViewModelCommand(ExecuteCheckDatabaseConnection);
    }

    // Метод для авторизации
    public void Authorized(Zamestnanec zamestnanec)
    {
        UserRole = zamestnanec.Role; // Устанавливаем роль пользователя
        CloseLoginWindow();

        var mainWindow = new MainWindow
        {
            DataContext = this // Передаем текущий NavigationVM
        };
        mainWindow.Show();
    }

    private void CloseLoginWindow()
    {
        foreach (Window window in Application.Current.Windows)
        {
            if (window is LoginView)
            {
                window.Close();
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
