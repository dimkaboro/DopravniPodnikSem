using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        private readonly ILogRepository _logRepository;
        private ObservableCollection<Log> _logy;
        private Log _selectedLog;
        private string _searchQuery;
        private Log _log;
        private string _errorMessage;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage)); 
            }
        }

        public ObservableCollection<Log> Logy
        {
            get => _logy;
            set
            {
                _logy = value;
                OnPropertyChanged();
            }
        }

        public Log SelectedLog
        {
            get => _selectedLog;
            set
            {
                _selectedLog = value;
                OnPropertyChanged();
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DeleteCommand { get; }

        public LogViewModel(ILogRepository logRepository)
        {
            _logRepository = logRepository;
            Logy = new ObservableCollection<Log>();

            SearchCommand = new ViewModelCommand(async _ => await SearchLogAsync(), _ => !string.IsNullOrEmpty(SearchQuery));
            ClearCommand = new ViewModelCommand(_ => ClearSearch());
            DeleteCommand = new ViewModelCommand(async _ => await DeleteLogAsync(), _ => SelectedLog != null);

            LoadAllLogyAsync();
        }

        public LogViewModel(Log log)
        {
            _log = log; 
        }

        private async void LoadAllLogyAsync()
        {
            var logy = await _logRepository.GetAllAsync();
            Logy = new ObservableCollection<Log>(logy);
        }

        private async System.Threading.Tasks.Task SearchLogAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchQuery))
                {
                    LoadAllLogyAsync();
                    return;
                }

                var allLogy = await _logRepository.GetAllAsync();
                var filteredLogy = allLogy
                    .Where(v => v.JakaTabulka.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filteredLogy.Count == 0)
                {
                    ErrorMessage = "Žádné záznamy nenalezeny.";
                }
                else
                {
                    ErrorMessage = string.Empty;
                }

                Logy = new ObservableCollection<Log>(filteredLogy);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při vyhledávání: {ex.Message}";
            }
        }

        private void ClearSearch()
        {
            SearchQuery = string.Empty;
            ErrorMessage = string.Empty;
            LoadAllLogyAsync();
        }

        private async System.Threading.Tasks.Task DeleteLogAsync()
        {
            if (SelectedLog != null)
            {
                await _logRepository.DeleteAsync(SelectedLog.LogId);
                LoadAllLogyAsync();
            }
        }
    }
}
