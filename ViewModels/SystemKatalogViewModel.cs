using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class SystemKatalogViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ISystemKatalogRepository _repository;
        private ObservableCollection<SystemKatalog> _systemKatalogList;
        private string _errorMessage;

        public ObservableCollection<SystemKatalog> SystemKatalogList
        {
            get => _systemKatalogList;
            set
            {
                _systemKatalogList = value;
                OnPropertyChanged(nameof(SystemKatalogList));
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

        public SystemKatalogViewModel(ISystemKatalogRepository repository)
        {
            _repository = repository;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                var data = await _repository.GetSystemKatalogAsync();
                SystemKatalogList = new ObservableCollection<SystemKatalog>(data);

                Console.WriteLine($"Data Loaded: {SystemKatalogList.Count} records");
                foreach (var item in SystemKatalogList)
                {
                    Console.WriteLine($"OWNER: {item.OWNER}, OBJECT_NAME: {item.OBJECT_NAME}");
                }

                ErrorMessage = string.Empty;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Error loading data: {ex.Message}";
                Console.WriteLine(ErrorMessage);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
