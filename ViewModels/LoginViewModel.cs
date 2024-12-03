using System;
using System.Windows;
using System.Windows.Input;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;

namespace DopravniPodnikSem.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _errorMessage;
        private readonly IUserDataRepository _userDataRepository;
        private readonly NavigationVM _navigation;

        public event Action RequestClose; // Событие для закрытия окна

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
                var user = await _userDataRepository.CheckCredentialsAsync(Email, Password);

                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                }
                else
                {
                    // Авторизация успешна
                    MessageBox.Show($"Welcome, {user.Jmeno}!");

                    // Сохраняем авторизованного пользователя в CurrentSession
                    CurrentSession.LoggedInUser = await _userDataRepository.GetUserDetailsAsync(user.ZamestnanecId);

                    // Передаем данные в NavigationVM
                    _navigation.Authorized(user);

                    // Старое окно пока не закрывается
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
