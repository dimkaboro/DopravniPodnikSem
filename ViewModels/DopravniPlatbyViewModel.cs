using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class DopravniPlatbyViewModel : BaseViewModel
    {
        private readonly IDopravniPlatbyRepository _repository;
        private readonly IJizdaRepository _jizdaRepository;
        private ObservableCollection<DopravniPlatba> _dopravniPlatby;
        private DopravniPlatba _selectedPlatba;
        private string _errorMessage;
        private readonly DatabaseService _databaseService;
        private DateTime? _searchDate;

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

        public DopravniPlatbyViewModel(IDopravniPlatbyRepository repository, IJizdaRepository jizdaRepository, DatabaseService databaseService)
        {
            _repository = repository;
            _jizdaRepository = jizdaRepository;
            _databaseService = databaseService;

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
                // Fetch the most frequent payment type and related data
                var mostFrequentTypeData = await _repository.GetMostFrequentPaymentTypeWithDetailsAsync();

                // Display the result in a popup message
                MessageBox.Show($"Most Frequent Payment Type: {mostFrequentTypeData.Type}\n" +
                                $"Count: {mostFrequentTypeData.Count}\n" +
                                $"Percentage: {mostFrequentTypeData.Percentage:F2}%",
                    "Result",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // Prompt the user to save the result to a CSV file
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv",
                    FileName = "MostFrequentPaymentTypeDetails.csv"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    // Save the result to the selected file
                    using (var writer = new StreamWriter(saveDialog.FileName))
                    {
                        // Write header
                        writer.WriteLine("Payment Type,Count,Percentage");

                        // Write the fetched data
                        writer.WriteLine($"{mostFrequentTypeData.Type},{mostFrequentTypeData.Count},{mostFrequentTypeData.Percentage:F2}");
                    }

                    // Notify the user about the successful save
                    MessageBox.Show($"The result has been successfully saved to the file: {saveDialog.FileName}",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    // Notify the user if the save operation was canceled
                    MessageBox.Show("The save operation was canceled by the user.",
                        "Information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                // Display any errors that occur during the process
                MessageBox.Show($"An error occurred while processing: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task SearchByDateAsync()
        {
            if (!SearchDate.HasValue)
            {
                ErrorMessage = "Vyberte prosím datum.";
                return;
            }

            try
            {
                var filtered = await _repository.GetByDateAsync(SearchDate.Value.Date);
                DopravniPlatby = new ObservableCollection<DopravniPlatba>(filtered);
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při hledání: {ex.Message}";
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
                ErrorMessage = "Chyba při načítání dat.";
            }
        }

        public async Task CalculateSummaryAsync()
        {
            try
            {
                var (totalCount, totalSum) = await _repository.CalculatePaymentSummaryAsync();

                MessageBox.Show($"Celkový počet plateb: {totalCount}, Celková částka: {totalSum:F2} CZK",
                    "Výsledek výpočtu", MessageBoxButton.OK, MessageBoxImage.Information);

                // Открываем диалог сохранения файла
                var saveFileDialog = new SaveFileDialog
                {
                    Title = "Save Payment Summary",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    FileName = "dopravni_platby_export.csv"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    var platby = await _repository.GetAllAsync();

                    // Сохраняем данные в выбранный файл
                    using (var writer = new StreamWriter(filePath))
                    {
                        writer.WriteLine("Cena,DatumNakupu,TypPlatby");

                        foreach (var platba in platby)
                        {
                            writer.WriteLine($"{platba.Cena},{platba.DatumNakupu:yyyy-MM-dd},{platba.TypPlatby}");
                        }

                        writer.WriteLine();
                        writer.WriteLine($"Total Count: {totalCount}, Total Sum: {totalSum:F2}");
                    }

                    MessageBox.Show($"Data successfully exported to: {filePath}", "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Export cancelled by the user.", "Export Cancelled", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při výpočtu nebo exportu: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddOrUpdateAsync()
        {
            try
            {
                if (SelectedPlatba == null)
                {
                    ErrorMessage = "Vyplňte prosím všechna pole před přidáním nebo aktualizací!";
                    return;
                }

                if (SelectedPlatba.BiletId == 0)
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
                        MessageBox.Show("Záznam úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    await _repository.UpdateAsync(SelectedPlatba);
                    MessageBox.Show("Záznam úspěšně aktualizován!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadDataAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při přidávání nebo aktualizaci záznamu: {ex.Message}";
            }
        }

        private async Task DeleteAsync()
        {
            try
            {
                await _repository.DeleteAsync(SelectedPlatba.BiletId);
                MessageBox.Show("Záznam úspěšně smazán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDataAsync();
            }
            catch
            {
                ErrorMessage = "Chyba při mazání záznamu.";
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

