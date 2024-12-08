using DopravniPodnikSem.Models;
using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Services;
using DopravniPodnikSem.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class ZamestnanecViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IAdresyRepository _adresyRepository;
        private readonly ISouboryRepository _souboryRepository;
        private readonly IConfiguration _configuration;

        private ObservableCollection<Zamestnanec> _zamestnanci;

        private Zamestnanec _selectedZamestnanec;
        private Adresa _selectedAdresa;
        private Soubory _selectedSoubor;

        private string _errorMessage;
        private Zamestnanec zamestnanec;
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
        public ObservableCollection<Zamestnanec> Zamestnanci
        {
            get => _zamestnanci;
            set
            {
                _zamestnanci = value;
                OnPropertyChanged();
            }
        }

        public Zamestnanec SelectedZamestnanec
        {
            get => _selectedZamestnanec;
            set
            {
                _selectedZamestnanec = value;
                OnPropertyChanged(nameof(SelectedZamestnanec));
            }
        }

        public Adresa SelectedAdresa
        {
            get => _selectedAdresa;
            set
            {
                _selectedAdresa = value;
                OnPropertyChanged(nameof(SelectedAdresa));
            }
        }

        public Soubory SelectedSoubor
        {
            get => _selectedSoubor;
            set
            {
                _selectedSoubor = value;
                OnPropertyChanged(nameof(SelectedSoubor));
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
        public ICommand ShowAddressCommand { get; }
        public ICommand ShowSouborCommand { get; }

        public ZamestnanecViewModel(IConfiguration configuration)
        {
            _userDataRepository = App.ServiceProvider.GetService<IUserDataRepository>();
            _adresyRepository = App.ServiceProvider.GetService<IAdresyRepository>();
            _souboryRepository = App.ServiceProvider.GetService<ISouboryRepository>();
            _configuration = configuration;

            UpdateCommand = new ViewModelCommand(async _ => await UpdateZamestnanecAsync(), _ => SelectedZamestnanec != null);
            ClearCommand = new ViewModelCommand(_ => ClearFields());
            ShowAddressCommand = new ViewModelCommand(ShowAddress);
            ShowSouborCommand = new ViewModelCommand(ShowSoubor);

            LoadDataAsync();
        }

        public ZamestnanecViewModel(Zamestnanec zamestnanec)
        {
            this.zamestnanec = zamestnanec;
        }


        private async void LoadDataAsync()
        {
            var zamestnanci = await _userDataRepository.GetAllAsync();
            Zamestnanci = new ObservableCollection<Zamestnanec>(zamestnanci);
        }

        private async Task UpdateZamestnanecAsync()
        {
            try
            {
                await _userDataRepository.UpdateAsync(SelectedZamestnanec);
                LoadDataAsync();
                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private async Task DeleteZamestnanecAsync()
        {
            try
            {
                await _userDataRepository.DeleteAsync(SelectedZamestnanec.ZamestnanecId);
                LoadDataAsync();
                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        private async void ShowAddress(object parameter)
        {
            await LoadAddressDetails();
            if (parameter is Zamestnanec zamestnanec && zamestnanec.AdresaId != null)
            {
                var adresaWindow = new AdresaWindow
                {
                    DataContext = new AdresaViewModel(App.ServiceProvider.GetService<IAdresyRepository>(), SelectedZamestnanec, SelectedAdresa)
                };

                adresaWindow.Show();
            }
            else
            {
                MessageBox.Show("Address not found for the selected employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task LoadAddressDetails()
        {
            try
            {
                var adresa = await _adresyRepository.GetAddressDetailsAsync(SelectedZamestnanec.AdresaId);
                SelectedAdresa = adresa;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private async void ShowSoubor(object parameter)
        {
            await LoadAvatarDetails();
            if (parameter is Zamestnanec zamestnanec && zamestnanec.SouborId != null)
            {
                var souborWindow = new SouborWindow
                {
                    DataContext = new SouboryViewModel(App.ServiceProvider.GetService<ISouboryRepository>(), SelectedZamestnanec, SelectedSoubor)
                };

                souborWindow.Show();
            }
            else
            {
                MessageBox.Show("Avatar not found for the selected employee.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task LoadAvatarDetails()
        {
            try
            {
                var soubor = await _souboryRepository.GetUserAvatarAsync(SelectedZamestnanec.SouborId);
                SelectedSoubor = soubor;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ClearFields()
        {
            SearchQuery = string.Empty; 
            SelectedZamestnanec = null; 
            LoadDataAsync();
            ErrorMessage = string.Empty; 
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
