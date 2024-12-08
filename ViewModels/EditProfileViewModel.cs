using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DopravniPodnikSem;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem.Views;

public class EditProfileViewModel : INotifyPropertyChanged
{
    private readonly IUserDataRepository _userDataRepository;
    private readonly IAdresyRepository _adresyRepository;
    private readonly ISouboryRepository _souboryRepository;

    private Zamestnanec _editedUser;
    private Adresa _editedAddress;
    private Soubory _currentSoubor;
    private byte[] _newAvatar;


    public EditProfileViewModel(IUserDataRepository userDataRepository, IAdresyRepository adresyRepository, ISouboryRepository souboryRepository, Zamestnanec currentUser, Adresa currentAddress, Soubory currentSoubor)
    {
        _userDataRepository = userDataRepository;
        _adresyRepository = adresyRepository;
        _souboryRepository = souboryRepository;

        EditedUser = new Zamestnanec
        {
            ZamestnanecId = currentUser.ZamestnanecId,
            Jmeno = currentUser.Jmeno,
            Prijmeni = currentUser.Prijmeni,
            Email = currentUser.Email,
            CisloTelefonu = currentUser.CisloTelefonu,
            AdresaId = currentUser.AdresaId,
            JePrivate = currentUser.JePrivate
        };

        EditedAddress = new Adresa
        {
            Mesto = currentAddress.Mesto,
            Ulice = currentAddress.Ulice,
            CisloBudovy = currentAddress.CisloBudovy,
            ZipCode = currentAddress.ZipCode,
            CisloBytu = currentAddress.CisloBytu
        };

        CurrentSoubor = currentSoubor;
    }

    public Zamestnanec EditedUser
    {
        get => _editedUser;
        set
        {
            _editedUser = value;
            OnPropertyChanged(nameof(EditedUser));
        }
    }

    public Adresa EditedAddress
    {
        get => _editedAddress;
        set
        {
            _editedAddress = value;
            OnPropertyChanged(nameof(EditedAddress));
        }
    }

    public Soubory CurrentSoubor
    {
        get => _currentSoubor;
        set
        {
            _currentSoubor = value;
            OnPropertyChanged(nameof(CurrentSoubor));
            OnPropertyChanged(nameof(CurrentAvatar)); 
        }
    }

    public byte[] NewAvatar
    {
        get => _newAvatar;
        set
        {
            _newAvatar = value;
            OnPropertyChanged(nameof(NewAvatar));
            OnPropertyChanged(nameof(CurrentAvatar));
        }
    }

    public byte[] CurrentAvatar => NewAvatar ?? CurrentSoubor?.Soubor;

    public async Task SaveChangesAsync()
    {
        try
        {
            if (NewAvatar != null)
            {
                var newAvatarId = await _souboryRepository.UpdateUserAvatarAsync(EditedUser.ZamestnanecId, "User avatar", NewAvatar);
                EditedUser.SouborId = newAvatarId; 
            }

            await _userDataRepository.UpdateEmployeeAsync(EditedUser);
            await _adresyRepository.UpdateAddressLogicAsync(EditedAddress, EditedUser.ZamestnanecId, EditedUser.AdresaId);

            await RefreshProfileAsync();

            var updatedUser = await _userDataRepository.GetUserDetailsAsync(EditedUser.ZamestnanecId);
            CurrentSession.LoggedInUser = updatedUser;

            MessageBox.Show("Data updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error updating data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public async Task RefreshProfileAsync()
    {
        try
        {
            var updatedUser = await _userDataRepository.GetUserDetailsAsync(EditedUser.ZamestnanecId);
            var updatedSoubor = await _souboryRepository.GetUserAvatarAsync(updatedUser.SouborId);

            EditedUser = updatedUser;
            CurrentSoubor = updatedSoubor;
            NewAvatar = null; 
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error refreshing profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void CancelChanges()
    {
        NewAvatar = null; 
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
