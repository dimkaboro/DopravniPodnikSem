using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkaViewModel : BaseViewModel
    {
        private readonly IZastavkaRepository _zastavkaRepository;

        private ObservableCollection<Zastavka> _zastavky;
        private Zastavka _selectedZastavka;
        private string _errorMessage;

        public ObservableCollection<Zastavka> Zastavky
        {
            get => _zastavky;
            set
            {
                _zastavky = value;
                OnPropertyChanged();
            }
        }

        public Zastavka SelectedZastavka
        {
            get => _selectedZastavka;
            set
            {
                _selectedZastavka = value;
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

        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public ZastavkaViewModel(IZastavkaRepository zastavkaRepository)
        {
            _zastavkaRepository = zastavkaRepository;

            AddCommand = new ViewModelCommand(async _ => await AddZastavkaAsync(), _ => SelectedZastavka != null);
            UpdateCommand = new ViewModelCommand(async _ => await UpdateZastavkaAsync(), _ => SelectedZastavka != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteZastavkaAsync(), _ => SelectedZastavka != null);
            ClearCommand = new ViewModelCommand(_ => ClearFields());

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var zastavky = await _zastavkaRepository.GetAllAsync();
            Zastavky = new ObservableCollection<Zastavka>(zastavky);

            // Добавляем пустую строку для ввода новых данных
            Zastavky.Add(new Zastavka
            {
                Nazev = string.Empty,
                GpsSouradnice = string.Empty
            });
        }

        private async Task AddZastavkaAsync()
        {
            await _zastavkaRepository.AddAsync(SelectedZastavka);
            LoadDataAsync();
        }

        private async Task UpdateZastavkaAsync()
        {
            await _zastavkaRepository.UpdateAsync(SelectedZastavka);
            LoadDataAsync();
        }

        private async Task DeleteZastavkaAsync()
        {
            await _zastavkaRepository.DeleteAsync(SelectedZastavka.ZastavkaId);
            LoadDataAsync();
        }

        private void ClearFields()
        {
            SelectedZastavka = new Zastavka();
            ErrorMessage = string.Empty;
        }
    }
}
