using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;
using DopravniPodnikSem.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using DopravniPodnikSem.Views;
using DopravniPodnikSem.Repository;
using DopravniPodnikSem.Services;
using System.Configuration;
using Microsoft.Extensions.Configuration;

public class SelectRealtionsViewModel : BaseViewModel
{
    private readonly ILinkyRepository _linkaRepository;
    private readonly IRidiciRepository _ridicRepository;
    private readonly IVozidloRepository _vozidloRepository;

    public ObservableCollection<Linka> Linky { get; private set; }
    public ObservableCollection<Ridic> Ridici { get; private set; }
    public ObservableCollection<Vozidlo> Vozidla { get; private set; }

    public Linka SelectedLinka { get; set; }
    public Ridic SelectedRidic { get; set; }
    public Vozidlo SelectedVozidlo { get; set; }

    public ICommand ConfirmCommand { get; }
    public ICommand CancelCommand { get; }

    public SelectRealtionsViewModel(
        ILinkyRepository linkaRepo,
        IRidiciRepository ridicRepo,
        IVozidloRepository vozidloRepo,
        IConfiguration configuration)
    {
        _linkaRepository = linkaRepo;
        _ridicRepository = ridicRepo;
        _vozidloRepository = vozidloRepo;

        LoadData();

        ConfirmCommand = new ViewModelCommand(_ => Confirm());
        CancelCommand = new ViewModelCommand(_ => Cancel());
    }

    private async void LoadData()
    {
        Linky = new ObservableCollection<Linka>(await _linkaRepository.GetAllAsync());
        Ridici = new ObservableCollection<Ridic>(await _ridicRepository.GetAllAsync());
        Vozidla = new ObservableCollection<Vozidlo>(await _vozidloRepository.GetAllAsync());

        OnPropertyChanged(nameof(Linky));
        OnPropertyChanged(nameof(Ridici));
        OnPropertyChanged(nameof(Vozidla));
    }

    private void Confirm()
    {
        if (SelectedLinka == null || SelectedRidic == null || SelectedVozidlo == null)
        {
            MessageBox.Show("Vyberte všechna požadovaná pole!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        var currentWindow = Application.Current.Windows
            .OfType<SelectRealtionsWindow>()
            .FirstOrDefault();

        if (currentWindow != null)
        {
            currentWindow.DialogResult = true;
            currentWindow.Close();
        }
    }

    private void Cancel()
    {
        var currentWindow = Application.Current.Windows
            .OfType<SelectRealtionsWindow>()
            .FirstOrDefault();

        currentWindow?.Close();
    }
}

