using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DopravniPodnikSem.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;

        private readonly Brush _highlightColor = (Brush)new BrushConverter().ConvertFrom("#6C63FF");
        public Brush Step1IndicatorColor => _currentStep == 1 ? _highlightColor : Brushes.DarkGray;
        public Brush Step2IndicatorColor => _currentStep == 2 ? _highlightColor : Brushes.DarkGray;
        public Brush Step3IndicatorColor => _currentStep == 3 ? _highlightColor : Brushes.DarkGray;

        private int _currentStep = 1;
        private UserControl _stepContent;
        private string _errorMessage;

        public string NextButtonText => _currentStep == 3 ? "Finish" : "Next";

        public UserControl StepContent
        {
            get => _stepContent;
            set
            {
                _stepContent = value;
                OnPropertyChanged(nameof(StepContent));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand NextCommand { get; }
        public ICommand BackCommand { get; }

        public bool IsBackVisible => _currentStep > 1;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostCode { get; set; }
        public string ApartmentNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegistrationViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            NextCommand = new ViewModelCommand(NextStep);
            BackCommand = new ViewModelCommand(BackStep);
            UpdateStepContent();
        }


        private void NextStep(object obj)
        {
            if (IsAnyFieldEmpty())
            {
                ErrorMessage = "Fill all fields";
                return;
            }

            switch (_currentStep)
            {
                case 1:
                    if (!IsValidPhoneNumber(PhoneNumber))
                    {
                        ErrorMessage = "Invalid phone number format. Example: +420123456789";
                        return;
                    }
                    break;

                case 2:
                    if (!IsValidHouseNumber(HouseNumber))
                    {
                        ErrorMessage = "House Number must be a number and no more than 4 digits.";
                        return;
                    }

                    if (!IsValidPostCode(PostCode))
                    {
                        ErrorMessage = "Post Code must be a number and no more than 6 digits.";
                        return;
                    }

                    if (!IsValidApartmentNumber(ApartmentNumber))
                    {
                        ErrorMessage = "Apartment Number must be a number and no more than 4 digits.";
                        return;
                    }
                    break;

                case 3:
                    if (!IsValidEmail(Email))
                    {
                        ErrorMessage = "Invalid email format.";
                        return;
                    }
                    break;
            }

            ErrorMessage = string.Empty;

            if (_currentStep < 3)
            {
                _currentStep++;
                UpdateStepContent();
            }
            else
            {
                CompleteRegistrationAsync();
            }
        }


        private void BackStep(object obj)
        {
            if (_currentStep > 1)
            {
                _currentStep--;
                UpdateStepContent();
            }
        }


        private bool IsAnyFieldEmpty()
        {
            return string.IsNullOrWhiteSpace(Name) ||
                   string.IsNullOrWhiteSpace(Surname) ||
                   string.IsNullOrWhiteSpace(PhoneNumber) ||
                   (_currentStep == 2 && (string.IsNullOrWhiteSpace(City) ||
                                          string.IsNullOrWhiteSpace(Street) ||
                                          string.IsNullOrWhiteSpace(HouseNumber) ||
                                          string.IsNullOrWhiteSpace(PostCode))) ||
                   (_currentStep == 3 && (string.IsNullOrWhiteSpace(Email) ||
                                          string.IsNullOrWhiteSpace(Password)));
        }


        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var phonePattern = @"^\+420\d{9}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }


        private bool IsValidHouseNumber(string houseNumber)
        {
            return int.TryParse(houseNumber, out _) && houseNumber.Length <= 4;
        }


        private bool IsValidPostCode(string postCode)
        {
            return int.TryParse(postCode, out _) && postCode.Length <= 6;
        }


        private bool IsValidApartmentNumber(string apartmentNumber)
        {
            return int.TryParse(apartmentNumber, out _) && apartmentNumber.Length <= 4;
        }


        private bool IsValidEmail(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }


        private void UpdateStepContent()
        {
            OnPropertyChanged(nameof(Step1IndicatorColor));
            OnPropertyChanged(nameof(Step2IndicatorColor));
            OnPropertyChanged(nameof(Step3IndicatorColor));
            OnPropertyChanged(nameof(IsBackVisible));
            OnPropertyChanged(nameof(NextButtonText));

            switch (_currentStep)
            {
                case 1:
                    StepContent = new Step1View();
                    break;
                case 2:
                    StepContent = new Step2View();
                    break;
                case 3:
                    StepContent = new Step3View();
                    break;
            }
        }


        private async Task CompleteRegistrationAsync()
        {
            try
            {
                var passwordService = new PasswordService();
                string hashedPassword = passwordService.HashPassword(Password);

                int addressID = await _databaseService.GetAddressIdAsync(City, Street, HouseNumber, PostCode, ApartmentNumber);

                if (addressID == 0)
                {
                    addressID = await _databaseService.AddAddressAsync(City, Street, HouseNumber, PostCode, ApartmentNumber);
                    await _databaseService.AddEmployeeAsync(Name, Surname, Email, hashedPassword, PhoneNumber, addressID);
                }
                else
                {
                    await _databaseService.AddEmployeeAsync(Name, Surname, Email, hashedPassword, PhoneNumber, addressID);
                }

                MessageBox.Show("Registration completed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
