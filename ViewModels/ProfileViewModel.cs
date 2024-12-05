using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DopravniPodnikSem;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly IUserDataRepository _userDataRepository;

    private Zamestnanec _currentUser;
    private Adresa _currentAdresa;
    private Soubory _currentSoubor;

    public ProfileViewModel(IUserDataRepository userDataRepository, int userId, int adresaId, int souborId)
    {
        _userDataRepository = userDataRepository;
        LoadUserDetails(userId, adresaId, souborId);
    }

    public Zamestnanec CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged(nameof(CurrentUser));
        }
    }

    public Adresa CurrentAdresa
    {
        get => _currentAdresa;
        set
        {
            _currentAdresa = value;
            OnPropertyChanged(nameof(CurrentAdresa));
        }
    }

    public Soubory CurrentSoubor
    {
        get => _currentSoubor;
        set
        {
            _currentSoubor = value;
            OnPropertyChanged(nameof(CurrentSoubor));
        }
    }

    private async void LoadUserDetails(int userId, int adresaId, int souborId)
    {
        try
        {
            var user = await _userDataRepository.GetUserDetailsAsync(userId);
            var adresa = await _userDataRepository.GetAddressDetailsAsync(adresaId);
            var soubor = await _userDataRepository.GetUserAvatarAsync(souborId);

            CurrentUser = user;
            CurrentAdresa = adresa;
            CurrentSoubor = soubor;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading user details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
