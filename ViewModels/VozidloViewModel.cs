using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DopravniPodnikSem.Repository;
using System.Windows;
using System.Text;

namespace DopravniPodnikSem.ViewModels
{
    public class VozidloViewModel : BaseViewModel
    {
        private readonly IVozidloRepository _vozidloRepository;
        private readonly ITypyVozidlaRepository _typyVozidlaRepository;

        private ObservableCollection<Vozidlo> _vozidla;
        private ObservableCollection<TypVozidla> _typyVozidla;

        private Vozidlo _selectedVozidlo;
        private string _searchQuery;
        private string _errorMessage;
        private string _procedureOutput;

        public VozidloViewModel(IVozidloRepository vozidloRepository, ITypyVozidlaRepository typyVozidlaRepository)
        {
            _vozidloRepository = vozidloRepository;
            _typyVozidlaRepository = typyVozidlaRepository;

            Vozidla = new ObservableCollection<Vozidlo>();
            TypyVozidla = new ObservableCollection<TypVozidla>();

            SearchCommand = new ViewModelCommand(async _ => await SearchVozidloAsync(), _ => !string.IsNullOrEmpty(SearchQuery));
            ClearCommand = new ViewModelCommand(_ => ClearSearch());
            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateVozidloAsync(), _ => SelectedVozidlo != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteVozidloAsync(), _ => SelectedVozidlo != null);
            GetVehicleCountsCommand = new ViewModelCommand(async _ => await GetVehicleCountsAsync());

            LoadAllVozidlaAsync();
            LoadTypyVozidlaAsync();
        }

        public string ProcedureOutput
        {
            get => _procedureOutput;
            set
            {
                _procedureOutput = value;
                OnPropertyChanged();
            }
        }


        private async Task GetVehicleCountsAsync()
        {
            try
            {
                var vehicleCounts = await _vozidloRepository.GetVehicleCountsByCapacityAsync();

                // Группируем данные по типу транспорта
                var groupedData = vehicleCounts
                    .GroupBy(vc => vc.VehicleType)
                    .Select(g => new
                    {
                        VehicleType = g.Key,
                        Ranges = g.Select(vc => $"{vc.CapacityRange}: {vc.Count}").ToList()
                    });

                // Формируем текст для отображения
                var message = new StringBuilder();
                foreach (var group in groupedData)
                {
                    message.AppendLine($"{group.VehicleType}:");
                    foreach (var range in group.Ranges)
                    {
                        message.AppendLine($"  {range}");
                    }
                }

                MessageBox.Show(message.ToString(), "Vehicle Counts by Capacity", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ObservableCollection<Vozidlo> Vozidla
        {
            get => _vozidla;
            set
            {
                _vozidla = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TypVozidla> TypyVozidla
        {
            get => _typyVozidla;
            set
            {
                _typyVozidla = value;
                OnPropertyChanged();
            }
        }

        public Vozidlo SelectedVozidlo
        {
            get => _selectedVozidlo;
            set
            {
                _selectedVozidlo = value;
                OnPropertyChanged();
                ErrorMessage = string.Empty; 
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

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GetVehicleCountsCommand { get; }

        private async void LoadAllVozidlaAsync()
        {
            try
            {
                var vozidla = await _vozidloRepository.GetAllAsync();
                Vozidla = new ObservableCollection<Vozidlo>(vozidla);
                ErrorMessage = string.Empty; 
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"ERROR: {ex.Message}");
            }
        }

        private async void LoadTypyVozidlaAsync()
        {
            try
            {
                var typy = await _typyVozidlaRepository.GetAllAsync();
                TypyVozidla = new ObservableCollection<TypVozidla>(typy);

                
                foreach (var typ in TypyVozidla)
                {
                    System.Diagnostics.Debug.WriteLine($"TypVozidlaId: {typ.TypVozidlaId}, Typ: {typ.Typ}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"ERROR: {ex.Message}");
            }
        }

        private async Task SearchVozidloAsync()
        {
            try
            {
                var vozidlo = await _vozidloRepository.GetByRegistrationNumberAsync(SearchQuery);
                Vozidla.Clear();

                if (vozidlo != null)
                {
                    Vozidla.Add(vozidlo);
                }
                else
                {
                    ShowErrorMessage("Not found.");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }

        private void ClearSearch()
        {
            SearchQuery = string.Empty;
            LoadAllVozidlaAsync();
        }

        private async Task AddOrUpdateVozidloAsync()
        {
            try
            {
                if (SelectedVozidlo != null)
                {
                    if (SelectedVozidlo.VozidloId == 0)
                    {
                        await _vozidloRepository.AddAsync(SelectedVozidlo);
                        ErrorMessage = "Data added successfully!";
                    }
                    else
                    {
                        await _vozidloRepository.UpdateAsync(SelectedVozidlo);
                        ErrorMessage = "Data updated successfully!";
                    }

                    LoadAllVozidlaAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"ERROR: {ex.Message}");
            }
        }

        private async Task DeleteVozidloAsync()
        {
            try
            {
                if (SelectedVozidlo != null)
                {
                    await _vozidloRepository.DeleteAsync(SelectedVozidlo.VozidloId);
                    ErrorMessage = "Data deleted successfully!";
                    LoadAllVozidlaAsync();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"ERROR: {ex.Message}");
            }
        }

        private void ShowErrorMessage(string message)
        {
            ErrorMessage = message;
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
