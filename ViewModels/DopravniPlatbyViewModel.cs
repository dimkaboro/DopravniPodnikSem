using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class DopravniPlatbyViewModel : BaseViewModel
    {
        private readonly IDopravniPlatbyRepository _repository;
        private readonly IJizdaRepository _jizdaRepository; // Add this line
        private ObservableCollection<DopravniPlatba> _dopravniPlatby;
        private DopravniPlatba _selectedPlatba;
        private string _errorMessage;
        private readonly DatabaseService _databaseService;
        private DateTime? _searchDate; // Change the type of _searchDate to DateTime?

        public DateTime? SearchDate
        {
            get => _searchDate;
            set
            {
                _searchDate = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DopravniPlatba> DopravniPlatby
        {
            get => _dopravniPlatby;
            set
            {
                _dopravniPlatby = value;
                OnPropertyChanged();
            }
        }

        public DopravniPlatba SelectedPlatba
        {
            get => _selectedPlatba;
            set
            {
                _selectedPlatba = value;
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

        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand CalculateSummaryCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand GetMostFrequentPaymentTypeCommand { get; }

        public DopravniPlatbyViewModel(IDopravniPlatbyRepository repository, IJizdaRepository jizdaRepository, DatabaseService databaseService) // Modify constructor
        {
            _repository = repository;
            _jizdaRepository = jizdaRepository; // Initialize the field
            _databaseService = databaseService; // Add this line

            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateAsync());
            DeleteCommand = new ViewModelCommand(async _ => await DeleteAsync(), _ => SelectedPlatba != null);
            ClearCommand = new ViewModelCommand(_ => ClearFields());
            CalculateSummaryCommand = new ViewModelCommand(async _ => await CalculateSummaryAsync());
            SearchCommand = new ViewModelCommand(async _ => await SearchByDateAsync(), _ => SearchDate.HasValue);
            GetMostFrequentPaymentTypeCommand = new ViewModelCommand(async _ => await ShowMostFrequentPaymentTypeAsync());

            LoadDataAsync();
        }

        private async Task ShowMostFrequentPaymentTypeAsync()
        {
            try
            {
                var mostFrequentType = await _repository.GetMostFrequentPaymentTypeAsync();
                MessageBox.Show($"Самый частый тип оплаты: {mostFrequentType}", "Результат", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении самого частого типа оплаты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                // Убедимся, что используется только дата без времени
                var filtered = await _repository.GetByDateAsync(SearchDate.Value.Date);
                DopravniPlatby = new ObservableCollection<DopravniPlatba>(filtered);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка при поиске: {ex.Message}";
            }
        }
        private async void LoadDataAsync()
        {
            try
            {
                var platby = await _repository.GetAllAsync();
                DopravniPlatby = new ObservableCollection<DopravniPlatba>(platby);
                ErrorMessage = string.Empty;
            }
            catch
            {
                ErrorMessage = "Ошибка загрузки данных.";
            }
        }

        public async Task CalculateSummaryAsync()
        {
            try
            {
                var (totalCount, totalSum) = await _repository.CalculatePaymentSummaryAsync();
                MessageBox.Show($"Всего платежей: {totalCount}, Суммарная сумма: {totalSum:F2} CZK",
                    "Результат расчёта", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчёте сводки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddOrUpdateAsync()
        {
            try
            {
                if (SelectedPlatba == null)
                {
                    ErrorMessage = "Пожалуйста, заполните все поля перед добавлением или обновлением!";
                    return;
                }

                if (SelectedPlatba.BiletId == 0) // Новая запись
                {
                    var typePlatbaView = new TypePlatbaView();
                    var typePlatbaViewModel = new TypePlatbaViewModel(_jizdaRepository);
                    typePlatbaView.DataContext = typePlatbaViewModel;

                    var dialogResult = typePlatbaView.ShowDialog();
                    if (dialogResult == true)
                    {
                        SelectedPlatba.TypPlatby = typePlatbaViewModel.SelectedType;
                        SelectedPlatba.JizdaJizdaId = typePlatbaViewModel.SelectedJizda.JizdaId;

                        await _repository.AddAsync(SelectedPlatba);
                        MessageBox.Show("Запись успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else // Обновление записи
                {
                    await _repository.UpdateAsync(SelectedPlatba);
                    MessageBox.Show("Запись успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadDataAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка добавления или обновления записи: {ex.Message}";
            }
        }

        private async Task DeleteAsync()
        {
            try
            {
                await _repository.DeleteAsync(SelectedPlatba.BiletId);
                MessageBox.Show("Запись успешно удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDataAsync();
            }
            catch
            {
                ErrorMessage = "Ошибка удаления записи.";
            }
        }

        private void ClearFields()
        {
            SearchDate = null;
            ErrorMessage = string.Empty;
            LoadDataAsync();
        }
    }
}

