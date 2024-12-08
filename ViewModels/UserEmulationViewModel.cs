using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DopravniPodnikSem;
using DopravniPodnikSem.Models;
using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using Microsoft.Extensions.DependencyInjection;

public class UserEmulationViewModel : INotifyPropertyChanged
{
    private readonly IUserDataRepository _userDataRepository;

    public ObservableCollection<Role> Roles { get; set; }

    private Role _selectedRole;
    public Role SelectedRole
    {
        get => _selectedRole;
        set
        {
            _selectedRole = value;
            OnPropertyChanged(nameof(SelectedRole));
            LoadUsersByRole(); 
        }
    }

    public ObservableCollection<Zamestnanec> Users { get; set; }
    public Zamestnanec SelectedUser { get; set; }

    public ICommand EmulateCommand { get; }

    public UserEmulationViewModel(IUserDataRepository userDataRepository)
    {
        _userDataRepository = userDataRepository;

        Roles = new ObservableCollection<Role>(Enum.GetValues(typeof(Role)).Cast<Role>());
        Users = new ObservableCollection<Zamestnanec>();

        EmulateCommand = new ViewModelCommand(ExecuteEmulateUser);
    }

    private async void LoadUsersByRole()
    {
        if (SelectedRole == 0) return;

        Users.Clear();
        var users = await _userDataRepository.GetAllUsersAsync(); 
        var filteredUsers = users.Where(user => user.Role == SelectedRole).ToList(); 

        foreach (var user in filteredUsers)
        {
            Users.Add(user);
        }
    }

    private void ExecuteEmulateUser(object parameter)
    {
        if (SelectedUser != null)
        {
            var navigationVM = App.ServiceProvider.GetService<NavigationVM>();
            navigationVM.EmulateUser(SelectedUser);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
