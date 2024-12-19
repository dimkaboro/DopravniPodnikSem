using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkyTrasyViewModel : BaseViewModel
    {
        private readonly IZastavkyTrasyRepository _zastavkyTrasyRepository;
        private readonly IJizdaRepository _jizdaRepository;
        private readonly IZastavkaRepository _zastavkaRepository;
        private ObservableCollection<ZastavkaTrasa> _zastavkyTrasy;
        private ZastavkaTrasa _selectedZastavkaTrasa;
        private string _searchCasPrijezdu;
        private string _errorMessage;

        public string SearchCasPrijezdu
        {
            get => _searchCasPrijezdu;
            set
            {
                _searchCasPrijezdu = value;
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

        public ObservableCollection<ZastavkaTrasa> ZastavkyTrasy
        {
            get => _zastavkyTrasy;
            set
            {
                _zastavkyTrasy = value;
                OnPropertyChanged();
            }
        }

        public ZastavkaTrasa SelectedZastavkaTrasa
        {
            get => _selectedZastavkaTrasa;
            set
            {
                _selectedZastavkaTrasa = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public ZastavkyTrasyViewModel(IZastavkyTrasyRepository zastavkyTrasyRepository, IJizdaRepository jizdaRepository, IZastavkaRepository zastavkaRepository)
        {
            _zastavkyTrasyRepository = zastavkyTrasyRepository;
            _jizdaRepository = jizdaRepository;
            _zastavkaRepository = zastavkaRepository;
            ZastavkyTrasy = new ObservableCollection<ZastavkaTrasa>();

            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateZastavkaTrasaAsync(), _ => SelectedZastavkaTrasa != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteZastavkaTrasaAsync(), _ => SelectedZastavkaTrasa != null);
            SearchCommand = new ViewModelCommand(async _ => await SearchByCasPrijezduAsync(), _ => !string.IsNullOrEmpty(SearchCasPrijezdu));
            ClearCommand = new ViewModelCommand(_ => ClearFields());

            LoadAllZastavkyTrasyAsync();
        }

        private async void LoadAllZastavkyTrasyAsync()
        {
            try
            {
                var zastavky = await _zastavkyTrasyRepository.GetAllAsync();
                ZastavkyTrasy.Clear();
                foreach (var zastavka in zastavky)
                    ZastavkyTrasy.Add(zastavka);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při načítání dat: {ex.Message}";
                Console.WriteLine(ex.Message);
            }
        }

        private async Task AddOrUpdateZastavkaTrasaAsync()
        {
            try
            {
                var addZastavkaTrasaView = new AddZastavkaTrasaView();

                var addZastavkaTrasaViewModel = new AddZastavkaTrasaViewModel(
                    _jizdaRepository,
                    _zastavkaRepository,
                    SelectedZastavkaTrasa 
                );

                addZastavkaTrasaView.DataContext = addZastavkaTrasaViewModel;

                var dialogResult = addZastavkaTrasaView.ShowDialog();
                if (dialogResult == true)
                {
                    LoadAllZastavkyTrasyAsync();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při přidání nebo aktualizaci záznamu: {ex.Message}";
            }
        }

        private async Task DeleteZastavkaTrasaAsync()
        {
            try
            {
                if (SelectedZastavkaTrasa != null)
                {
                    Console.WriteLine($"The data with ID is deleted: {SelectedZastavkaTrasa.ZastavkaTrasaId}");

                    await _zastavkyTrasyRepository.DeleteAsync(SelectedZastavkaTrasa.ZastavkaTrasaId);

                    ZastavkyTrasy.Remove(SelectedZastavkaTrasa);

                    LoadAllZastavkyTrasyAsync();

                    SelectedZastavkaTrasa = null; 
                    ErrorMessage = string.Empty;
                }
                else
                {
                    ErrorMessage = "Vyberte řádek k odstranění.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Chyba při mazání: {ex.Message}";
                Console.WriteLine(ex.Message);
            }
        }


        private async Task SearchByCasPrijezduAsync()
        {
            try
            {
                Console.WriteLine($"SearchByCasPrijezduAsync called with: {SearchCasPrijezdu}");

                if (!string.IsNullOrWhiteSpace(SearchCasPrijezdu))
                {
                    var filtered = (await _zastavkyTrasyRepository.GetAllAsync())
                        .Where(z => z.CasPrijezdu.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)
                        .Contains(SearchCasPrijezdu.ToLower()))
                        .ToList();

                    ZastavkyTrasy.Clear();
                    foreach (var zastavka in filtered)
                        ZastavkyTrasy.Add(zastavka);

                    if (!filtered.Any())
                    {
                        ErrorMessage = "Nebyly nalezeny žádné záznamy.";
                    }
                    else
                    {
                        ErrorMessage = string.Empty;
                    }
                }
                else
                {
                    ErrorMessage = "Zadejte čas pro vyhledávání.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                Console.WriteLine(ex.Message);
            }
        }

        private void ClearFields()
        {
            SearchCasPrijezdu = string.Empty;
            SelectedZastavkaTrasa = null;
            ZastavkyTrasy.Clear();
            LoadAllZastavkyTrasyAsync();
            ErrorMessage = string.Empty;
        }
    }
}
