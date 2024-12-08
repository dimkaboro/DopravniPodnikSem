using System;
using System.Windows;
using System.Windows.Input;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DopravniPodnikSem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private readonly IUserDataRepository _userDataRepository;
        private readonly NavigationVM _navigation;

        public event Action RequestClose; 

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
            _navigation = App.ServiceProvider.GetService<NavigationVM>();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private async void ExecuteLoginCommand(object obj)
        {
            try
            {
                var user = await _userDataRepository.CheckCredentialsAsync(Email, Password);

                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                }
                else
                {
                    MessageBox.Show($"Welcome, {user.Jmeno}!");

                    CurrentSession.LoggedInUser = await _userDataRepository.GetUserDetailsAsync(user.ZamestnanecId);

                    _navigation.Authorized(user);

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error during login: {ex.Message}";
            }
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}
