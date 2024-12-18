using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using DopravniPodnikSem.Views;
using DopravniPodnikSem.ViewModels;

namespace DopravniPodnikSem.ViewModels
{
    public class AddZastavkaTrasaViewModel : BaseViewModel
    {
        private readonly IJizdaRepository _jizdaRepository;
        private readonly IZastavkaRepository _zastavkaRepository;

        private Jizda _selectedJizda;
        private Zastavka _selectedZastavka;

        private ObservableCollection<Jizda> _jizdy;
        private ObservableCollection<Zastavka> _zastavky;

        public Jizda SelectedJizda
        {
            get => _selectedJizda;
            set
            {
                _selectedJizda = value;
                OnPropertyChanged();
            }
        }

        public Zastavka SelectedZastavka
        {
            get => _selectedZastavka;
            set
            {
                _selectedZastavka = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Jizda> Jizdy
        {
            get => _jizdy;
            set
            {
                _jizdy = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Zastavka> Zastavky
        {
            get => _zastavky;
            set
            {
                _zastavky = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        //public ICommand OpenAddViewCommand { get; }

        public AddZastavkaTrasaViewModel(IJizdaRepository jizdaRepository, IZastavkaRepository zastavkaRepository)
        {
            _jizdaRepository = jizdaRepository;
            _zastavkaRepository = zastavkaRepository;

            LoadData();

            //OpenAddViewCommand = new ViewModelCommand(_ => OpenAddView());
            ConfirmCommand = new ViewModelCommand(param => Confirm());
            CancelCommand = new ViewModelCommand(param => Cancel());
        }

        //private void OpenAddView()
        //{
        //    var addWindow = new AddZastavkaTrasaView
        //    {
        //        DataContext = new AddZastavkaTrasaViewModel(_jizdaRepository, _zastavkaRepository)
        //    };

        //    if (addWindow.ShowDialog() == true)
        //    {
        //        MessageBox.Show("Запись добавлена успешно.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        //        LoadAllZastavkyTrasyAsync();
        //    }
        //}

        private async void LoadData()
        {
            var jizdy = await _jizdaRepository.GetAllAsync();
            Jizdy = new ObservableCollection<Jizda>(jizdy);


            var zastavky = await _zastavkaRepository.GetAllAsync();
            Zastavky = new ObservableCollection<Zastavka>(zastavky);
        }

        private void Confirm()
        {
            if (SelectedJizda == null || SelectedZastavka == null)
            {
                MessageBox.Show("Vyberte prosím Jizdu a Zastavku.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var currentWindow = Application.Current.Windows.OfType<AddZastavkaTrasaView>().FirstOrDefault();
            if (currentWindow?.DataContext is ZastavkyTrasyViewModel parentViewModel)
            {
                parentViewModel.SelectedZastavkaTrasa.JizdaId = SelectedJizda.JizdaId;
                parentViewModel.SelectedZastavkaTrasa.ZastavkaId = SelectedZastavka.ZastavkaId;
            }

            currentWindow.DialogResult = true;
            currentWindow.Close();
        }

        private void Cancel()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
    }
}
