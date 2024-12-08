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
    private readonly DatabaseService _databaseService;
    private object _currentView;
    private Role? _userRole;
    public Zamestnanec EmulatedUser { get; private set; }


    public bool IsRoleInfoVisible => UserRole != null; 
    public bool IsEmulating => EmulatedUser != null;
    public bool NotEmulating => !IsEmulating;


    public ICommand StopEmulationCommand { get; }


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

    public NavigationVM(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        CurrentView = new HomeView();
        StopEmulationCommand = new ViewModelCommand(ExecuteStopEmulation);
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

    public void Authorized(Zamestnanec zamestnanec)
    {
        UserRole = zamestnanec.Role; 
        
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

        CurrentView = new HomeView();

        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow)
            {
                window.Close();
                break;
            }
        }
    }

    public void Logout()
    {
        CurrentSession.LoggedInUser = null;
        CurrentSession.OriginalUser = null;
        EmulatedUser = null;
        UserRole = null;

        var guestWindow = new MainWindow();
        guestWindow.Show();

        CurrentView = new HomeView();

        foreach (Window window in Application.Current.Windows)
        {
            if (window is EmployeeWindow || window is AdminWindow)
            {
                window.Close();
                break;
            }
        }
    }

    public void EmulateUser(Zamestnanec userToEmulate)
    {
        EmulatedUser = userToEmulate;
        CurrentSession.OriginalUser = CurrentSession.LoggedInUser;
        CurrentSession.LoggedInUser = EmulatedUser;
        UserRole = EmulatedUser.Role;

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
        else if (UserRole == Role.Guest)
        {
            var guestWindow = new MainWindow();
            guestWindow.Show();
        }

        CurrentView = new HomeView();

        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow || window is EmployeeWindow || window is AdminWindow)
            {
                window.Close();
                break;
            }
        }
    }

    private void ExecuteStopEmulation(object parameter)
    {
        EmulatedUser = null;
        CurrentSession.LoggedInUser = CurrentSession.OriginalUser;
        CurrentSession.OriginalUser = null;
        UserRole = CurrentSession.LoggedInUser.Role;

        var adminWindow = new AdminWindow();
        adminWindow.Show();

        CurrentView = new HomeView();

        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow || window is EmployeeWindow || window is AdminWindow)
            {
                window.Close();
                break;
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
