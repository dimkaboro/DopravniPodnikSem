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
        private string _searchNazev;

        public string SearchNazev
        {
            get => _searchNazev;
            set
            {
                _searchNazev = value;
                OnPropertyChanged();
            }
        }

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

      
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddUpdateCommand { get; }

        public ZastavkaViewModel(IZastavkaRepository zastavkaRepository)
        {
            _zastavkaRepository = zastavkaRepository;

            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateZastavkaAsync(), _ => SelectedZastavka != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteZastavkaAsync(), _ => SelectedZastavka != null);
            ClearCommand = new ViewModelCommand(_ => ClearSelection());
            SearchCommand = new ViewModelCommand(async _ => await SearchZastavkyAsync(), _ => !string.IsNullOrWhiteSpace(SearchNazev));

            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                var zastavky = await _zastavkaRepository.GetAllAsync();
                Zastavky = new ObservableCollection<Zastavka>(zastavky);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки данных: {ex.Message}";
            }
        }

        private async Task AddOrUpdateZastavkaAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedZastavka?.Nazev) || string.IsNullOrWhiteSpace(SelectedZastavka?.GpsSouradnice))
                {
                    ErrorMessage = "Пожалуйста, заполните все поля.";
                    return;
                }

                if (SelectedZastavka.ZastavkaId == 0) // Если ID отсутствует, добавляем новую запись
                {
                    await _zastavkaRepository.AddAsync(SelectedZastavka);
                    ErrorMessage = "Запись успешно добавлена.";
                }
                else // Если ID существует, обновляем запись
                {
                    await _zastavkaRepository.UpdateAsync(SelectedZastavka);
                    ErrorMessage = "Запись успешно обновлена.";
                }

                // Обновляем данные
                LoadDataAsync();

                // Сбрасываем выбранную запись
                SelectedZastavka = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private async Task SearchZastavkyAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchNazev))
                {
                    // Если поле поиска пустое, загружаем весь список
                    LoadDataAsync();
                    return;
                }

                // Получаем все данные из базы
                var allZastavky = await _zastavkaRepository.GetAllAsync();

                // Фильтруем данные по совпадению с поисковым запросом
                var filteredZastavky = allZastavky
                    .Where(z => z.Nazev.Contains(SearchNazev, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (!filteredZastavky.Any())
                {
                    ErrorMessage = "Žádné záznamy nenalezeny.";
                }
                else
                {
                    ErrorMessage = string.Empty;
                }

                // Обновляем коллекцию для отображения
                Zastavky = new ObservableCollection<Zastavka>(filteredZastavky);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při vyhledávání: {ex.Message}";
            }
        }

        private async Task DeleteZastavkaAsync()
        {
            if (SelectedZastavka == null)
            {
                ErrorMessage = "Пожалуйста, выберите запись для удаления.";
                return;
            }

            try
            {
                await _zastavkaRepository.DeleteAsync(SelectedZastavka.ZastavkaId);
                LoadDataAsync();
                ClearSelection();
                ErrorMessage = "Запись успешно удалена.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка удаления записи: {ex.Message}";
            }
        }

        private void ClearSelection()
        {
            SelectedZastavka = null;
            SearchNazev = string.Empty;
            LoadDataAsync();
        }
    }
}
