using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Microsoft.Extensions.Configuration;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class JizdaViewModel : BaseViewModel
    {
        private readonly IJizdaRepository _jizdaRepository;
        private readonly IConfiguration _configuration;

        private ObservableCollection<Jizda> _jizdy;
        private Jizda _selectedJizda;
        private string _searchStav;
        private string _durationResult;
        private string _errorMessage;
        private Jizda jizda;
        private string _searchQuery;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Jizda> Jizdy
        {
            get => _jizdy;
            set
            {
                _jizdy = value;
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

        public string SearchStav
        {
            get => _searchStav;
            set
            {
                _searchStav = value;
                OnPropertyChanged();
            }
        }

        public string DurationResult
        {
            get => _durationResult;
            set
            {
                _durationResult = value;
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
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand UpdateStatusesCommand { get; }
        public ICommand CalculateDurationCommand { get; }
        public ICommand GetLongestJizdaCommand { get; }

        public JizdaViewModel(IJizdaRepository jizdaRepository, IConfiguration configuration)
        {
            _jizdaRepository = jizdaRepository;
            _configuration = configuration;

            AddCommand = new ViewModelCommand(async _ => await AddJizdaAsync());
            UpdateCommand = new ViewModelCommand(async _ => await UpdateJizdaAsync(), _ => SelectedJizda != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteJizdaAsync(), _ => SelectedJizda != null);
            SearchCommand = new ViewModelCommand(async _ => await SearchJizdyAsync(), _ => !string.IsNullOrEmpty(SearchQuery));
            ClearCommand = new ViewModelCommand(_ => ClearFields());
            UpdateStatusesCommand = new ViewModelCommand(async _ => await UpdateStatusesAsync());
            CalculateDurationCommand = new ViewModelCommand(async _ => await CalculateDurationAsync(), _ => SelectedJizda != null && SelectedJizda.JizdaId > 0);
            GetLongestJizdaCommand = new ViewModelCommand(async _ => await ShowLongestJizdaAsync()); // Новая команда

            LoadDataAsync();
        }

        private async Task ShowLongestJizdaAsync()
        {
            try
            {
                // Вызов метода GetLongestJizdaAsync из репозитория
                var (linkaNazev, casOd, casDo, duration) = await _jizdaRepository.GetLongestJizdaAsync();

                // Формирование и отображение сообщения с результатом
                MessageBox.Show(
                    $"Самая долгая поездка:\n" +
                    $"Маршрут: {linkaNazev}\n" +
                    $"Начало: {casOd:dd.MM.yyyy HH:mm:ss}\n" +
                    $"Окончание: {casDo:dd.MM.yyyy HH:mm:ss}\n" +
                    $"Длительность: {duration.TotalHours:F2} часов",
                    "Самая долгая поездка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
            catch (Exception ex)
            {
                // Обработка ошибок и уведомление
                MessageBox.Show($"Ошибка при получении самой долгой поездки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public JizdaViewModel(Jizda jizda)
        {
            this.jizda = jizda;
        }

        private async void LoadDataAsync()
        {
            var jizdy = await _jizdaRepository.GetAllAsync();
            Jizdy = new ObservableCollection<Jizda>(jizdy);
        }

        private async Task AddJizdaAsync()
        {
            try
            {
                // Проверяем, заданы ли значения CasOd и CasDo
                if (SelectedJizda?.CasOd == default || SelectedJizda?.CasDo == default)
                {
                    ErrorMessage = "Zadejte čas od a čas do před přidáním!";
                    return;
                }

                // Создаем ViewModel для выбора связей
                var selectRelationsViewModel = new SelectRealtionsViewModel(
                    new LinkyRepository(new DatabaseService(_configuration)),
                    new RidiciRepository(new DatabaseService(_configuration)),
                    new VozidloRepository(new DatabaseService(_configuration)),
                    _configuration
                );

                // Создаем окно выбора связей
                var selectRelationsWindow = new SelectRealtionsWindow
                {
                    DataContext = selectRelationsViewModel
                };

                // Показываем окно модально
                var result = selectRelationsWindow.ShowDialog();

                if (result == true)
                {
                    // Проверяем, выбраны ли все связи
                    if (selectRelationsViewModel.SelectedLinka == null ||
                        selectRelationsViewModel.SelectedRidic == null ||
                        selectRelationsViewModel.SelectedVozidlo == null)
                    {
                        ErrorMessage = "Vyberte všechna požadovaná pole!";
                        return;
                    }

                    // Создаем новую запись Jizda
                    var newJizda = new Jizda
                    {
                        CasOd = SelectedJizda.CasOd,
                        CasDo = SelectedJizda.CasDo,
                        LinkaId = selectRelationsViewModel.SelectedLinka.LinkaId,
                        RidicId = selectRelationsViewModel.SelectedRidic.RidicId,
                        VozidloId = selectRelationsViewModel.SelectedVozidlo.VozidloId
                    };

                    // Добавляем запись через репозиторий
                    await _jizdaRepository.AddAsync(newJizda);

                    // Обновляем данные
                    LoadDataAsync();

                    // Уведомление об успешном добавлении
                    ErrorMessage = string.Empty;
                    MessageBox.Show("Nový záznam byl úspěšně přidán do databáze.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba: {ex.Message}";
            }
        }

        private async Task UpdateJizdaAsync()
        {
            try
            {
                await _jizdaRepository.UpdateAsync(SelectedJizda);
                LoadDataAsync();
                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private async Task DeleteJizdaAsync()
        {
            try
            {
                await _jizdaRepository.DeleteAsync(SelectedJizda.JizdaId);
                LoadDataAsync();
                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }

        private async Task SearchJizdyAsync()
        {
            try
            {
                // Проверяем корректность формата даты
                if (DateTime.TryParseExact(SearchQuery, "M/d/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var searchDate))
                {
                    // Выполняем поиск по дате
                    var filteredJizdy = await _jizdaRepository.GetByDateAsync(searchDate.Date);

                    if (filteredJizdy.Any())
                    {
                        Jizdy = new ObservableCollection<Jizda>(filteredJizdy);
                        ErrorMessage = string.Empty; // Очищаем ошибку, если записи найдены
                    }
                    else
                    {
                        Jizdy = new ObservableCollection<Jizda>();
                        ErrorMessage = "Žádné záznamy nenalezeny."; // Если ничего не найдено
                    }
                }
                else
                {
                    // Если формат даты некорректен
                    ErrorMessage = "Zadejte platné datum ve formátu MM/DD/YYYY.";
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку и отображаем сообщение
                ErrorMessage = $"Chyba při vyhledávání: {ex.Message}";
            }
        }

        private async Task UpdateStatusesAsync()
        {
            try
            {
                await _jizdaRepository.UpdateStatusesAsync();
                LoadDataAsync();
                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Ошибка: {ex.Message}";
            }
        }



        private async Task CalculateDurationAsync()
        {
            if (SelectedJizda == null) return;

            var duration = await _jizdaRepository.CalculateDurationAsync(SelectedJizda.JizdaId);
            MessageBox.Show($"Duration: {duration}", "Duration", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearFields()
        {
            SearchQuery = string.Empty; // Очистка текстового поля поиска
            SelectedJizda = null; // Снятие выделения в таблице
            LoadDataAsync();
            ErrorMessage = string.Empty; // Сброс сообщения об ошибке
            DurationResult = string.Empty; // Очистка результатов расчёта
        }
    }
}
