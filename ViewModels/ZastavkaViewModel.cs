using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkaViewModel : BaseViewModel
    {
        private Zastavka _zastavka;

        public ZastavkaViewModel(Zastavka zastavka)
        {
            _zastavka = zastavka;
        }

        public int ZastavkaId
        {
            get => _zastavka.ZastavkaId;
            set
            {
                _zastavka.ZastavkaId = value;
                OnPropertyChanged();
            }
        }

        public string Nazev
        {
            get => _zastavka.Nazev;
            set
            {
                _zastavka.Nazev = value;
                OnPropertyChanged();
            }
        }

        public string GpsSouradnice
        {
            get => _zastavka.GpsSouradnice;
            set
            {
                _zastavka.GpsSouradnice = value;
                OnPropertyChanged();
            }
        }
    }
}
