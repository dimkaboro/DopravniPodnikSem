﻿using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DopravniPodnikSem.ViewModels
{
    public class VozidloViewModel : BaseViewModel
    {
        private readonly IVozidloRepository _vozidloRepository;
        private ObservableCollection<Vozidlo> _vozidla;
        private Vozidlo _vozidlo; 
        private Vozidlo _selectedVozidlo;
        private string _searchQuery;

        public VozidloViewModel(Vozidlo vozidlo)
        {
            _vozidlo = vozidlo; 
        }

        public VozidloViewModel(IVozidloRepository vozidloRepository)
        {
            _vozidloRepository = vozidloRepository;
            Vozidla = new ObservableCollection<Vozidlo>();

            SearchCommand = new ViewModelCommand(async _ => await SearchVozidloAsync(), _ => !string.IsNullOrEmpty(SearchQuery));
            ClearCommand = new ViewModelCommand(_ => ClearSearch());
            AddUpdateCommand = new ViewModelCommand(async _ => await AddOrUpdateVozidloAsync(), _ => SelectedVozidlo != null);
            DeleteCommand = new ViewModelCommand(async _ => await DeleteVozidloAsync(), _ => SelectedVozidlo != null);

            LoadAllVozidlaAsync();
        }

        public ObservableCollection<Vozidlo> Vozidla
        {
            get => _vozidla;
            set
            {
                _vozidla = value;
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
        public ICommand ClearCommand { get; }
        public ICommand AddUpdateCommand { get; }
        public ICommand DeleteCommand { get; }

        private async void LoadAllVozidlaAsync()
        {
            var vozidla = await _vozidloRepository.GetAllAsync();
            Vozidla = new ObservableCollection<Vozidlo>(vozidla);
        }

        private async System.Threading.Tasks.Task SearchVozidloAsync()
        {
            var vozidlo = await _vozidloRepository.GetByRegistrationNumberAsync(SearchQuery);
            Vozidla.Clear();

            if (vozidlo != null)
            {
                Vozidla.Add(vozidlo);
            }
        }

        private void ClearSearch()
        {
            SearchQuery = string.Empty;
            LoadAllVozidlaAsync();
        }

        private async System.Threading.Tasks.Task AddOrUpdateVozidloAsync()
        {
            if (SelectedVozidlo.VozidloId == 0)
            {
                await _vozidloRepository.AddAsync(SelectedVozidlo);
            }
            else
            {
                await _vozidloRepository.UpdateAsync(SelectedVozidlo);
            }

            LoadAllVozidlaAsync();
        }

        private async System.Threading.Tasks.Task DeleteVozidloAsync()
        {
            if (SelectedVozidlo != null)
            {
                await _vozidloRepository.DeleteAsync(SelectedVozidlo.VozidloId);
                LoadAllVozidlaAsync();
            }
        }
    }
}
