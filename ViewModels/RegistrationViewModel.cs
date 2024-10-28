using DopravniPodnikSem.Views;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DopravniPodnikSem.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private readonly Brush _highlightColor = (Brush)new BrushConverter().ConvertFrom("#6C63FF");
        public Brush Step1IndicatorColor => _currentStep == 1 ? _highlightColor : Brushes.DarkGray;
        public Brush Step2IndicatorColor => _currentStep == 2 ? _highlightColor : Brushes.DarkGray;
        public Brush Step3IndicatorColor => _currentStep == 3 ? _highlightColor : Brushes.DarkGray;

        private int _currentStep = 1;
        private UserControl _stepContent;

        public string NextButtonText
        {
            get => _currentStep == 3 ? "Finish" : "Next";
        }

        public UserControl StepContent
        {
            get => _stepContent;
            set
            {
                _stepContent = value;
                OnPropertyChanged(nameof(StepContent));
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

        public RegistrationViewModel()
        {
            NextCommand = new ViewModelCommand(NextStep);
            BackCommand = new ViewModelCommand(BackStep);
            UpdateStepContent();
        }

        private void NextStep(object obj)
        {
            if (_currentStep < 3)
            {
                _currentStep++;
                UpdateStepContent();
            }
            else
            {
                // Завершение регистрации
                // Здесь добавьте логику для сохранения данных, например, в базу данных.
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
