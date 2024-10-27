using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace DopravniPodnikSem.ViewModels
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _phoneNumber;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public ICommand CancelCommand { get; }
        public ICommand NextCommand { get; }

        public RegistrationViewModel()
        {
            CancelCommand = new ViewModelCommand(param => Cancel());
            NextCommand = new ViewModelCommand(param => Next());
        }

        private void Cancel()
        {
            // Реализация логики для Cancel, например, закрытие окна
            MessageBox.Show("Registration cancelled.");
        }

        private void Next()
        {
            // Реализация логики для Next, например, переход к следующему шагу
            MessageBox.Show("Proceeding to the next step.");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
