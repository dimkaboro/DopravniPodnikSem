using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository.Interfaces;

namespace DopravniPodnikSem.ViewModels
{
    public class GarazViewModel : BaseViewModel
    {
        private Garaz _garaz;
        private readonly IVozidloRepository _vozidloRepository;
        private readonly ITypyVozidlaRepository _typyVozidlaRepository;

        public GarazViewModel(Garaz garaz, IVozidloRepository vozidloRepository, ITypyVozidlaRepository typyVozidlaRepository)
        {
            _garaz = garaz;
            _vozidloRepository = vozidloRepository;
            _typyVozidlaRepository = typyVozidlaRepository;
        }

        public int GarazId
        {
            get => _garaz.GarazId;
            set
            {
                _garaz.GarazId = value;
                OnPropertyChanged();
            }
        }

        public string Nazev
        {
            get => _garaz.Nazev;
            set
            {
                _garaz.Nazev = value;
                OnPropertyChanged();
            }
        }

        public string Adresa
        {
            get => _garaz.Adresa;
            set
            {
                _garaz.Adresa = value;
                OnPropertyChanged();
            }
        }

        public ZamestnanecViewModel ZamestnanecViewModel => new ZamestnanecViewModel(_garaz.Zamestnanec);

        public IEnumerable<VozidloViewModel> VozidlaViewModel =>
            _garaz.Vozidla?.Select(v => new VozidloViewModel(_vozidloRepository, _typyVozidlaRepository));
    }
}
