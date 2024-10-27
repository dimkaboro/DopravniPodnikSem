using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Net;
using System.Security;
using System.Windows.Input;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;

//namespace DopravniPodnikSem.ViewModels
//{
//    public class LoginViewModel : BaseViewModel
//    {
//        private string _userName;
//        private SecureString _password;
//        private string _errorMessage;
//        private UserData _userData;

//        private readonly NavigationVM _navigation;
//        private readonly IUserDataRepository _userDataRepository;

//        private bool _isLoggingIn;
//        public bool IsLoggingIn
//        {
//            get { return _isLoggingIn; }
//            set
//            {
//                if (_isLoggingIn != value)
//                {
//                    _isLoggingIn = value;
//                    OnPropertyChanged(nameof(IsLoggingIn));
//                }
//            }
//        }

//        public UserData User
//        {
//            get { return _userData; }
//            set
//            {
//                _userData = value;
//                OnPropertyChanged(nameof(User));
//            }
//        }

//        public string UserName
//        {
//            get { return _userName; }
//            set
//            {
//                _userName = value;
//                OnPropertyChanged(nameof(UserName));
//            }
//        }

//        public SecureString Password
//        {
//            get { return _password; }
//            set
//            {
//                _password = value;
//                OnPropertyChanged(nameof(Password));
//            }
//        }

//        public string ErrorMessage
//        {
//            get { return _errorMessage; }
//            set
//            {
//                _errorMessage = value;
//                OnPropertyChanged(nameof(ErrorMessage));
//            }
//        }

//        public ICommand LoginCommand { get; }

//        public LoginViewModel(IUserDataRepository userDataRepository, NavigationVM navigation)
//        {
//            _userDataRepository = userDataRepository;
//            _navigation = navigation;
//            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
//        }

//        private async void ExecuteLoginCommand(object obj)
//        {
//            try
//            {
//                IsLoggingIn = true;

//                UserData user = await Task.Run(() =>
//                    _userDataRepository.CheckCredentials(new NetworkCredential(UserName, Password)));
//                if (user == null)
//                {
//                    ErrorMessage = "* Неверное имя пользователя или пароль";
//                }
//                else
//                {
//                    _navigation.Authorized(user);
//                }
//            }
//            finally
//            {
//                IsLoggingIn = false;
//            }
//        }

//        private bool CanExecuteLoginCommand(object obj)
//        {
//            return !string.IsNullOrWhiteSpace(UserName) && UserName.Length >= 3 &&
//                   Password != null && Password.Length >= 3;
//        }
//    }
//}
