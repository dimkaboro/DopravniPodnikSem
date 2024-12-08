using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class SearchVozidloViewModel : BaseViewModel
    {
        private readonly IVozidloRepository _vozidloRepository;
        private readonly Action<Vozidlo> _onVozidloSelected;

        private ObservableCollection<Vozidlo> _vozidla;
        private ObservableCollection<Vozidlo> _filteredVozidla; 
        private Vozidlo _selectedVozidlo;
        private string _searchQuery;

        public ObservableCollection<Vozidlo> FilteredVozidla
        {
            get => _filteredVozidla;
            set
            {
                _filteredVozidla = value;
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

        public ICommand SearchCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        public SearchVozidloViewModel(IVozidloRepository vozidloRepository, Action<Vozidlo> onVozidloSelected)
        {
            _vozidloRepository = vozidloRepository;
            _onVozidloSelected = onVozidloSelected;

            SearchCommand = new ViewModelCommand(_ => SearchVozidla());
            AddCommand = new ViewModelCommand(param => AddSelectedVozidlo(param), _ => SelectedVozidlo != null);
            CancelCommand = new ViewModelCommand(_ => CancelSelection());

            LoadVozidlaAsync();
        }

        private async void LoadVozidlaAsync()
        {
            var vozidla = await _vozidloRepository.GetAllAsync();
            _vozidla = new ObservableCollection<Vozidlo>(vozidla);
            FilteredVozidla = new ObservableCollection<Vozidlo>(_vozidla);
        }

        private void SearchVozidla()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredVozidla = new ObservableCollection<Vozidlo>(_vozidla);
                return;
            }

            FilteredVozidla = new ObservableCollection<Vozidlo>(
                _vozidla.Where(v => v.RegistracniCislo.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)));
        }

        private void AddSelectedVozidlo(object parameter)
        {
            if (parameter is Window currentWindow) 
            {
                _onVozidloSelected?.Invoke(SelectedVozidlo); 
                currentWindow.Close(); 
            }
            else
            {
                throw new InvalidOperationException("Expected a Window parameter.");
            }
        }

        private void CancelSelection()
        {
            _onVozidloSelected?.Invoke(null);
        }
    }
}
