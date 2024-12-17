using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class TypePlatbaViewModel : BaseViewModel
    {
        private readonly IJizdaRepository _jizdaRepository;

        private string _selectedType;
        private Jizda _selectedJizda;
        private ObservableCollection<string> _paymentTypes;
        private ObservableCollection<Jizda> _jizdaList;

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        public Jizda SelectedJizda
        {
            get => _selectedJizda;
            set
            {
                _selectedJizda = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> PaymentTypes
        {
            get => _paymentTypes;
            set
            {
                _paymentTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Jizda> JizdaList
        {
            get => _jizdaList;
            set
            {
                _jizdaList = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public TypePlatbaViewModel(IJizdaRepository jizdaRepository)
        {
            _jizdaRepository = jizdaRepository;

            PaymentTypes = new ObservableCollection<string> { "Hotovost", "Karta", "Faktura" };
            LoadJizdaList();

            ConfirmCommand = new ViewModelCommand(param => Confirm());
            CancelCommand = new ViewModelCommand(param => Cancel());
        }

        private async void LoadJizdaList()
        {
            var jizdy = await _jizdaRepository.GetAllAsync();
            JizdaList = new ObservableCollection<Jizda>(jizdy);
        }

        private void Confirm()
        {
            if (string.IsNullOrEmpty(SelectedType) || SelectedJizda == null)
            {
                MessageBox.Show("Vyberte prosím typ platby a cestu.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var currentWindow = Application.Current.Windows.OfType<TypePlatbaView>().FirstOrDefault();
            if (currentWindow?.DataContext is DopravniPlatbyViewModel parentViewModel)
            {
                parentViewModel.SelectedPlatba.TypPlatby = SelectedType;
                parentViewModel.SelectedPlatba.JizdaJizdaId = SelectedJizda.JizdaId;
            }

            currentWindow.DialogResult = true;
            currentWindow.Close();
        }

        private void Cancel()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
    }
}


