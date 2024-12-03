using System.ComponentModel;
using System.Threading.Tasks;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly IUserDataRepository _userDataRepository;

    private Zamestnanec _currentUser;
    private Adresa _currentAdresa;
    private byte[] _currentSoubor;

    public ProfileViewModel(IUserDataRepository userDataRepository, int userId, int adresaId)
    {
        _userDataRepository = userDataRepository;
        LoadUserDetails(userId, adresaId);
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

    public byte[] CurrentSoubor
    {
        get => _currentSoubor;
        set
        {
            _currentSoubor = value;
            OnPropertyChanged(nameof(CurrentSoubor));
        }
    }

    private async void LoadUserDetails(int userId, int adresaId)
    {
        try
        {
            var user = await _userDataRepository.GetUserDetailsAsync(userId);
            var adresa = await _userDataRepository.GetAddressDetailsAsync(adresaId);
            var soubor = await _userDataRepository.GetUserAvatarAsync(user.SouborId);

            CurrentUser = user;
            CurrentAdresa = adresa;
            CurrentSoubor = soubor;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user details: {ex.Message}");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
