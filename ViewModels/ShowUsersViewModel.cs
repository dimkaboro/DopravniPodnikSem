using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using DopravniPodnikSem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using DopravniPodnikSem.Views;

public class ShowUsersViewModel : INotifyPropertyChanged
{
    private readonly IUserDataRepository _userDataRepository;
    private readonly IAdresyRepository _adresyRepository;
    private readonly ISouboryRepository _souboryRepository;
    private NavigationVM _navigationVM;

    public ObservableCollection<Zamestnanec> Users { get; set; }
    private Zamestnanec _selectedUser;
    private Adresa _selectedAdresa;
    private Soubory _selectedSoubor;

    public ICommand ShowProfileCommand { get; }
    public ICommand ReturnCommand { get; }

    public string DisplayPhoneNumber =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedUser?.CisloTelefonu
            : "***";

    public string DisplayCity =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedAdresa?.Mesto
            : "***";

    public string DisplayStreet =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedAdresa?.Ulice
            : "***";

    public string DisplayHouseNumber =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedAdresa?.CisloBudovy
            : "***";

    public string DisplayPostalCode =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedAdresa?.ZipCode
            : "***";

    public string DisplayApartmentNumber =>
        CurrentSession.LoggedInUser.Role == Role.Administrator || SelectedUser?.JePrivate == 0
            ? SelectedAdresa?.CisloBytu
            : "***";


    public ShowUsersViewModel()
    {
        _userDataRepository = App.ServiceProvider.GetService<IUserDataRepository>();
        _adresyRepository = App.ServiceProvider.GetService<IAdresyRepository>();
        _souboryRepository = App.ServiceProvider.GetService<ISouboryRepository>();
        _navigationVM = App.ServiceProvider.GetService<NavigationVM>();

        Users = new ObservableCollection<Zamestnanec>();

        ShowProfileCommand = new ViewModelCommand(ExecuteShowProfile);
        ReturnCommand = new ViewModelCommand(ExecuteReturn);

        LoadUsers();
    }

    public Zamestnanec SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (_selectedUser != value)
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                OnPropertyChanged(nameof(DisplayPhoneNumber));
            }
        }
    }

    public Adresa SelectedAdresa
    {
        get => _selectedAdresa;
        set
        {
            if (_selectedAdresa != value)
            {
                _selectedAdresa = value;
                OnPropertyChanged(nameof(SelectedAdresa));
                OnPropertyChanged(nameof(DisplayCity));
                OnPropertyChanged(nameof(DisplayStreet));
                OnPropertyChanged(nameof(DisplayHouseNumber));
                OnPropertyChanged(nameof(DisplayPostalCode));
                OnPropertyChanged(nameof(DisplayApartmentNumber));
            }
        }
    }

    public Soubory SelectedSoubor
    {
        get => _selectedSoubor;
        set
        {
            if (_selectedSoubor != value)
            {
                _selectedSoubor = value;
                OnPropertyChanged(nameof(SelectedSoubor));
            }
        }
    }

    private async void LoadUsers()
    {
        Users.Clear();
        var users = await _userDataRepository.GetEmployeeHierarchyAsync();

        foreach (var user in users)
        {
            Users.Add(user);
        }
    }

    private async void ExecuteShowProfile(object parameter)
    {
        if (SelectedUser != null)
        {
            var adresa = await _adresyRepository.GetAddressDetailsAsync(SelectedUser.AdresaId);
            var soubor = await _souboryRepository.GetUserAvatarAsync(SelectedUser.SouborId);

            SelectedAdresa = adresa;
            SelectedSoubor = soubor;

            _navigationVM.CurrentView = new ShowUserView
            {
                DataContext = this
            };
        }
    }

    private void ExecuteReturn(object parameter)
    {
        SelectedUser = null;
        SelectedAdresa = null;
        SelectedSoubor = null;

        _navigationVM.CurrentView = new ShowUsersView
        {
            DataContext= this
        };
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

