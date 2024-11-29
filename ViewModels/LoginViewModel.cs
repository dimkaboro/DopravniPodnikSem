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
using System.Windows;

namespace DopravniPodnikSem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private readonly IUserDataRepository _userDataRepository;
        private readonly NavigationVM _navigation;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
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
                // Проверяем учетные данные
                var user = await _userDataRepository.CheckCredentialsAsync(Email, Password);

                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                }
                else
                {
                    // Авторизация успешна
                    MessageBox.Show("Authorization completed successfully!");
                    _navigation.Authorized(user); // Перенаправляем пользователя после авторизации
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during login: {ex.Message}";
            }
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            // Команда активна только при заполненных полях
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
