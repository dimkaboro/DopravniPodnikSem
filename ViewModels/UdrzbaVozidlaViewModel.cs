using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DopravniPodnikSem.Repository;


namespace DopravniPodnikSem.ViewModels
{
    public class UdrzbaVozidlaViewModel : BaseViewModel
    {
        private readonly IUdrzbaVozidlaRepository _udrzbaVozidlaRepository;
        private readonly IVozidloRepository _vozidloRepository;

        private ObservableCollection<UdrzbaVozidla> _udrzbyVozidla;
        private UdrzbaVozidla _selectedUdrzbaVozidla;
        private DateTime? _searchDate;
        private string _errorMessage;

        public ObservableCollection<UdrzbaVozidla> UdrzbyVozidla
        {
            get => _udrzbyVozidla;
            set
            {
                _udrzbyVozidla = value;
                OnPropertyChanged();
            }
        }

        public UdrzbaVozidla SelectedUdrzbaVozidla
        {
            get => _selectedUdrzbaVozidla;
            set
            {
                _selectedUdrzbaVozidla = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SearchDate
        {
            get => _searchDate;
            set
            {
                _searchDate = value;
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
        public ICommand SearchByDateCommand { get; }

        public UdrzbaVozidlaViewModel(IUdrzbaVozidlaRepository udrzbaVozidlaRepository, IVozidloRepository vozidloRepository)
        {
            _udrzbaVozidlaRepository = udrzbaVozidlaRepository;
            _vozidloRepository = vozidloRepository;

            AddCommand = new ViewModelCommand(async _ => await AddUdrzbaAsync(), _ => SelectedUdrzbaVozidla != null);
            UpdateCommand = new ViewModelCommand(async _ => await OpenUpdateSearchWindowAsync(), _ => SelectedUdrzbaVozidla != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteUdrzbaAsync(), _ => SelectedUdrzbaVozidla != null);
            ClearCommand = new ViewModelCommand(_ => ClearFields());
            SearchByDateCommand = new ViewModelCommand(async _ => await SearchByDateAsync(), _ => SearchDate.HasValue);

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var udrzby = await _udrzbaVozidlaRepository.GetAllAsync();
            UdrzbyVozidla = new ObservableCollection<UdrzbaVozidla>(udrzby);
        }

        private async Task SearchByDateAsync()
        {
            if (!SearchDate.HasValue)
            {
                ErrorMessage = "Пожалуйста, выберите дату.";
                return;
            }

            try
            {
                var filtered = await _udrzbaVozidlaRepository.GetAllByDateAsync(SearchDate.Value);
                UdrzbyVozidla = new ObservableCollection<UdrzbaVozidla>(filtered);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при поиске: {ex.Message}";
            }
        }

        private async Task AddUdrzbaAsync()
        {
            if (string.IsNullOrEmpty(SelectedUdrzbaVozidla?.Popis) || SelectedUdrzbaVozidla.DatumUdrzby == default)
            {
                ErrorMessage = "Пожалуйста, заполните все поля.";
                return;
            }

            var searchViewModel = new SearchVozidloViewModel(_vozidloRepository, async selectedVozidlo =>
            {
                if (selectedVozidlo != null)
                {
                    await _udrzbaVozidlaRepository.AddAsync(SelectedUdrzbaVozidla, selectedVozidlo.VozidloId);
                    LoadDataAsync();
                }
            });

            var searchWindow = new SearchVozidloWindow
            {
                DataContext = searchViewModel
            };

            searchWindow.ShowDialog();
        }

        private async Task OpenUpdateSearchWindowAsync()
        {
            if (SelectedUdrzbaVozidla == null)
            {
                ErrorMessage = "Выберите запись для обновления.";
                return;
            }

            var vozidla = await _vozidloRepository.GetAllAsync();
            var currentVozidlo = vozidla.FirstOrDefault(v => v.UdrzbaVozidlaUdrzbaId == SelectedUdrzbaVozidla.UdrzbaId);

            var searchViewModel = new SearchVozidloViewModel(_vozidloRepository, async selectedVozidlo =>
            {
                if (selectedVozidlo != null)
                {
                    await _udrzbaVozidlaRepository.UpdateAsync(SelectedUdrzbaVozidla, selectedVozidlo.VozidloId);
                    LoadDataAsync();
                }
            });

            var searchWindow = new SearchVozidloWindow
            {
                DataContext = searchViewModel
            };

            if (currentVozidlo != null)
            {
                searchViewModel.SelectedVozidlo = currentVozidlo;
            }

            searchWindow.ShowDialog();
        }

        private async Task DeleteUdrzbaAsync()
        {
            if (SelectedUdrzbaVozidla == null)
            {
                ErrorMessage = "Выберите запись для удаления.";
                return;
            }

            try
            {
                var vozidla = await _vozidloRepository.GetAllAsync();
                var currentVozidlo = vozidla.FirstOrDefault(v => v.UdrzbaVozidlaUdrzbaId == SelectedUdrzbaVozidla.UdrzbaId);

                if (currentVozidlo != null)
                {
                    await _udrzbaVozidlaRepository.DeleteAsync(SelectedUdrzbaVozidla.UdrzbaId, currentVozidlo.VozidloId);
                    LoadDataAsync();
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Не удалось найти связанное транспортное средство.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при удалении: {ex.Message}";
            }
        }

        private void ClearFields()
        {
            SelectedUdrzbaVozidla = null;
            SearchDate = null;
            ErrorMessage = string.Empty;
            LoadDataAsync();
        }
    }
}
