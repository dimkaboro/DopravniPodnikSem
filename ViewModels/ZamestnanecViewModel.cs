using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class ZamestnanecViewModel : BaseViewModel
    {
        private Zamestnanec _zamestnanec;

        public ZamestnanecViewModel(Zamestnanec zamestnanec)
        {
            _zamestnanec = zamestnanec;
        }

        public int ZamestnanecId
        {
            get => _zamestnanec.ZamestnanecId;
            set
            {
                _zamestnanec.ZamestnanecId = value;
                OnPropertyChanged();
            }
        }

        public string Jmeno
        {
            get => _zamestnanec.Jmeno;
            set
            {
                _zamestnanec.Jmeno = value;
                OnPropertyChanged();
            }
        }

        public string Prijmeni
        {
            get => _zamestnanec.Prijmeni;
            set
            {
                _zamestnanec.Prijmeni = value;
                OnPropertyChanged();
            }
        }

        public string Pozice
        {
            get => _zamestnanec.Pozice;
            set
            {
                _zamestnanec.Pozice = value;
                OnPropertyChanged();
            }
        }

        public decimal Plat
        {
            get => _zamestnanec.Plat;
            set
            {
                _zamestnanec.Plat = value;
                OnPropertyChanged();
            }
        }

        public DateTime DatumNastupu
        {
            get => _zamestnanec.DatumNastupu;
            set
            {
                _zamestnanec.DatumNastupu = value;
                OnPropertyChanged();
            }
        }
    }
}
