using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class LinkaViewModel : BaseViewModel
    {
        private readonly ILinkyRepository _linkyRepository;
        private ObservableCollection<Linka> _linky;
        private Linka _selectedLinka;
        private string _errorMessage;
        private string _searchNazev;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public string SearchNazev
        {
            get => _searchNazev;
            set
            {
                _searchNazev = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Linka> Linky
        {
            get => _linky;
            set
            {
                _linky = value;
                OnPropertyChanged();
            }
        }

        public Linka SelectedLinka
        {
            get => _selectedLinka;
            set
            {
                _selectedLinka = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand SearchCommand { get; }

        public LinkaViewModel(ILinkyRepository linkyRepository)
        {
            _linkyRepository = linkyRepository;
            Linky = new ObservableCollection<Linka>();

            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateLinkaAsync(), _ => SelectedLinka != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteLinkaAsync(), _ => SelectedLinka != null);
            ClearCommand = new ViewModelCommand(_ => ClearSelection());
            SearchCommand = new ViewModelCommand(async _ => await SearchLinkyAsync());

            LoadAllLinkyAsync();
        }

        private async void LoadAllLinkyAsync()
        {
            try
            {
                var linky = await _linkyRepository.GetAllAsync();
                Linky = new ObservableCollection<Linka>(linky);
                ErrorMessage = string.Empty;
            }
            catch
            {
                ErrorMessage = "Error";
            }
        }

        private async Task AddOrUpdateLinkaAsync()
        {
            try
            {
                if (SelectedLinka.LinkaId == 0)
                {
                    await _linkyRepository.AddAsync(SelectedLinka);
                }
                else
                {
                    await _linkyRepository.UpdateAsync(SelectedLinka);
                }

                LoadAllLinkyAsync();
                ErrorMessage = string.Empty;
            }
            catch
            {
                ErrorMessage = "Error";
            }
        }

        private async Task SearchLinkyAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchNazev))
            {
                LoadAllLinkyAsync(); 
                return;
            }

            try
            {
                var linky = await _linkyRepository.GetAllAsync();
                var filteredLinky = linky.Where(l => l.Nazev.Contains(SearchNazev, System.StringComparison.OrdinalIgnoreCase)).ToList();
                Linky = new ObservableCollection<Linka>(filteredLinky);

                if (!Linky.Any())
                {
                    ErrorMessage = "Error";
                }
                else
                {
                    ErrorMessage = string.Empty;
                }
            }
            catch
            {
                ErrorMessage = "Error";
            }
        }

        private async Task DeleteLinkaAsync()
        {
            try
            {
                if (SelectedLinka != null)
                {
                    await _linkyRepository.DeleteAsync(SelectedLinka.LinkaId);
                    LoadAllLinkyAsync();
                    ErrorMessage = string.Empty;
                }
            }
            catch
            {
                ErrorMessage = "Error";
            }
        }

        private void ClearSelection()
        {
            SelectedLinka = null;
        }
    }
}
