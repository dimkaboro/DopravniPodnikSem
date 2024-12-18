using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkyTrasyViewModel : BaseViewModel
    {
        private readonly IZastavkyTrasyRepository _zastavkyTrasyRepository;
        private ObservableCollection<ZastavkaTrasa> _zastavkyTrasy;
        private ZastavkaTrasa _selectedZastavkaTrasa;
        private string _searchCasPrijezdu;
        private string _errorMessage;

        // Свойства
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

        // Команды
        public ICommand SearchCommand { get; }
        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        // Конструктор
        public ZastavkyTrasyViewModel(IZastavkyTrasyRepository zastavkyTrasyRepository)
        {
            _zastavkyTrasyRepository = zastavkyTrasyRepository;
            ZastavkyTrasy = new ObservableCollection<ZastavkaTrasa>();

            // Привязка команд
            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateZastavkaTrasaAsync(), _ => SelectedZastavkaTrasa != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteZastavkaTrasaAsync(), _ => SelectedZastavkaTrasa != null);
            SearchCommand = new ViewModelCommand(async _ => await SearchByCasPrijezduAsync(), _ => !string.IsNullOrEmpty(SearchCasPrijezdu));
            ClearCommand = new ViewModelCommand(_ => ClearFields());

            LoadAllZastavkyTrasyAsync();
        }

        // Загрузка всех данных
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

        // Добавление или обновление записи
        private async Task AddOrUpdateZastavkaTrasaAsync()
        {
            try
            {
                if (SelectedZastavkaTrasa.ZastavkaTrasaId == 0)
                {
                    await _zastavkyTrasyRepository.AddAsync(SelectedZastavkaTrasa);
                }
                else
                {
                    await _zastavkyTrasyRepository.UpdateAsync(SelectedZastavkaTrasa);
                }
                LoadAllZastavkyTrasyAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        // Удаление записи
        private async Task DeleteZastavkaTrasaAsync()
        {
            try
            {
                if (SelectedZastavkaTrasa != null)
                {
                    Console.WriteLine($"Удаляется запись с ID: {SelectedZastavkaTrasa.ZastavkaTrasaId}");

                    // Удаление записи из базы данных
                    await _zastavkyTrasyRepository.DeleteAsync(SelectedZastavkaTrasa.ZastavkaTrasaId);

                    // Удаление из локальной коллекции
                    ZastavkyTrasy.Remove(SelectedZastavkaTrasa);

                    // Перезагрузка всей коллекции на случай рассинхронизации
                    LoadAllZastavkyTrasyAsync();

                    SelectedZastavkaTrasa = null; // Сброс выделенной записи
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


        // Поиск по времени CasPrijezdu
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

        // Очистка полей и перезагрузка данных
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
