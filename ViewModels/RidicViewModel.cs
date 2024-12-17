using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DopravniPodnikSem.Repository;
using System.Windows;

namespace DopravniPodnikSem.ViewModels
{
    public class RidiciViewModel : BaseViewModel
    {
        private readonly IRidiciRepository _ridiciRepository;
        private ObservableCollection<Ridic> _ridici;
        private Ridic _selectedRidic;
        private string _errorMessage;
        private string _searchPrijmeni;

        public string SearchPrijmeni
        {
            get => _searchPrijmeni;
            set
            {
                _searchPrijmeni = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ObservableCollection<Ridic> Ridici
        {
            get => _ridici;
            set
            {
                _ridici = value;
                OnPropertyChanged();
            }
        }

        public Ridic SelectedRidic
        {
            get => _selectedRidic;
            set
            {
                _selectedRidic = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand HowManyBirthdaysCommand { get; }
        public ICommand ClearCommand { get; }

        public RidiciViewModel(IRidiciRepository ridiciRepository)
        {
            _ridiciRepository = ridiciRepository;
            Ridici = new ObservableCollection<Ridic>();

            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateRidicAsync(), _ => SelectedRidic != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteRidicAsync(), _ => SelectedRidic != null);
            SearchCommand = new ViewModelCommand(async _ => await SearchRidicByLastNameAsync(), _ => !string.IsNullOrEmpty(SearchPrijmeni));
            ClearCommand = new ViewModelCommand(_ => ClearFields());
            HowManyBirthdaysCommand = new ViewModelCommand(async _ => await ShowBirthdaysAsync());

            LoadAllRidiciAsync();
        }

        private async Task ShowBirthdaysAsync()
        {
            var birthdays = await _ridiciRepository.GetBirthdaysInCurrentMonthAsync();

            if (birthdays.Any())
            {
                string message = string.Join("\n", birthdays);
                MessageBox.Show(message, "Narozeniny v tomto měsíci", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("V tomto měsíci nejsou žádné narozeniny.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void LoadAllRidiciAsync()
        {
            var ridici = await _ridiciRepository.GetAllAsync();
            Ridici = new ObservableCollection<Ridic>(ridici);
        }

        private async Task AddOrUpdateRidicAsync()
        {
            if (SelectedRidic.RidicId == 0)
            {
                await _ridiciRepository.AddAsync(SelectedRidic);
            }
            else
            {
                await _ridiciRepository.UpdateAsync(SelectedRidic);
            }

            LoadAllRidiciAsync();
        }

        private async Task SearchRidicByLastNameAsync()
        {
            try
            {
                var ridici = await _ridiciRepository.GetByLastNameAsync(SearchPrijmeni);
                Ridici.Clear();

                if (ridici != null)
                {
                    foreach (var ridic in ridici)
                        Ridici.Add(ridic);
                }

                ErrorMessage = string.Empty;
            }
            catch
            {
                ErrorMessage = "Error";
            }
        }

        private void ClearFields()
        {
            SearchPrijmeni = string.Empty;  
            SelectedRidic = null;           
            Ridici.Clear();                 
            LoadAllRidiciAsync();           
            ErrorMessage = string.Empty;    
        }

        private async Task DeleteRidicAsync()
        {
            if (SelectedRidic != null)
            {
                await _ridiciRepository.DeleteAsync(SelectedRidic.RidicId);
                LoadAllRidiciAsync();
            }
        }
    }
}
