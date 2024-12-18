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
using Microsoft.Extensions.DependencyInjection;

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

        private ZastavkaTrasa _currentZastavkaTrasa;

        public ZastavkaTrasa CurrentZastavkaTrasa
        {
            get => _currentZastavkaTrasa;
            set
            {
                _currentZastavkaTrasa = value;
                OnPropertyChanged();
            }
        }

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



        public AddZastavkaTrasaViewModel(IJizdaRepository jizdaRepository, IZastavkaRepository zastavkaRepository, ZastavkaTrasa existingZastavkaTrasa = null)
        {
            _jizdaRepository = jizdaRepository;
            _zastavkaRepository = zastavkaRepository;

            // Инициализация CurrentZastavkaTrasa для нового объекта
            CurrentZastavkaTrasa = existingZastavkaTrasa ?? new ZastavkaTrasa();

            LoadData();

            ConfirmCommand = new ViewModelCommand(param => Confirm());
            CancelCommand = new ViewModelCommand(param => Cancel());

            // Если передана существующая запись, используем её для редактирования
            if (existingZastavkaTrasa != null)
            {
                SelectedJizda = new Jizda { JizdaId = existingZastavkaTrasa.JizdaId };
                SelectedZastavka = new Zastavka { ZastavkaId = existingZastavkaTrasa.ZastavkaId };
            }
        }



        private async void LoadData()
        {
            var jizdy = await _jizdaRepository.GetAllAsync();
            Jizdy = new ObservableCollection<Jizda>(jizdy);


            var zastavky = await _zastavkaRepository.GetAllAsync();
            Zastavky = new ObservableCollection<Zastavka>(zastavky);
        }

        private async void Confirm()
        {
            if (SelectedJizda == null || SelectedZastavka == null)
            {
                MessageBox.Show("Vyberte prosím Jizdu a Zastavku.", "ERROR", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var repository = App.ServiceProvider.GetService<IZastavkyTrasyRepository>();

                if (CurrentZastavkaTrasa.ZastavkaTrasaId == 0)
                {
                    // Добавление новой записи
                    var zastavkaTrasa = new ZastavkaTrasa
                    {
                        JizdaId = SelectedJizda.JizdaId,
                        ZastavkaId = SelectedZastavka.ZastavkaId,
                        CasPrijezdu = DateTime.Now
                    };

                    await repository.AddAsync(zastavkaTrasa);
                    MessageBox.Show("Záznam úspěšně přidán!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обновление существующей записи
                    CurrentZastavkaTrasa.JizdaId = SelectedJizda.JizdaId;
                    CurrentZastavkaTrasa.ZastavkaId = SelectedZastavka.ZastavkaId;
                    CurrentZastavkaTrasa.CasPrijezdu = DateTime.Now;

                    await repository.UpdateAsync(CurrentZastavkaTrasa);
                    MessageBox.Show("Záznam úspěšně aktualizován!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                CloseWindow(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            currentWindow?.Close();
        }
        private void CloseWindow(bool result)
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
            if (currentWindow != null)
            {
                currentWindow.DialogResult = result;
                currentWindow.Close();
            }
        }
    }
}
