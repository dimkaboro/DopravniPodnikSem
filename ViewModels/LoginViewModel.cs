using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Net;
using System.Security;
using System.Windows.Input;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DopravniPodnikSem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
private SecureString _password;
private string _errorMessage;
private readonly IUserDataRepository _userDataRepository;
private readonly NavigationVM _navigation;

public string UserName
{
    get => _userName;
    set
    {
        _userName = value;
        OnPropertyChanged();
    }
}

public SecureString Password
{
    get => _password;
    set
    {
        _password = value;
        OnPropertyChanged();
    }
}

public string ErrorMessage
{
    get => _errorMessage;
    set
    {
        _errorMessage = value;
        OnPropertyChanged();
    }
}

public ICommand LoginCommand { get; }

public LoginViewModel(IUserDataRepository userDataRepository, NavigationVM navigation)
{
    _userDataRepository = userDataRepository;
    _navigation = navigation;
    LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
}

private async void ExecuteLoginCommand(object obj)
{
    try
    {
        UserData user = await Task.Run(() => _userDataRepository.CheckCredentials(new NetworkCredential(UserName, Password)));
        if (user == null)
        {
            ErrorMessage = "Неверное имя пользователя или пароль";
        }
        else
        {
            _navigation.Authorized(user);
        }
    }
    catch
    {
        ErrorMessage = "Ошибка при попытке подключения";
    }
}

private bool CanExecuteLoginCommand(object obj)
{
    return !string.IsNullOrWhiteSpace(UserName) && Password != null && Password.Length > 0;
}
    }
}
