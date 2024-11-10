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

        public string Email
        {
            get => _zamestnanec.Email;
            set
            {
                _zamestnanec.Email = value;
                OnPropertyChanged();
            }
        }

        public string Heslo
        {
            get => _zamestnanec.Heslo;
            set
            {
                _zamestnanec.Heslo = value;
                OnPropertyChanged();
            }
        }

        public string CisloTelefonu
        {
            get => _zamestnanec.CisloTelefonu;
            set
            {
                _zamestnanec.CisloTelefonu = value;
                OnPropertyChanged();
            }
        }

        public string Mesto
        {
            get => _zamestnanec.Mesto;
            set
            {
                _zamestnanec.Mesto = value;
                OnPropertyChanged();
            }
        }

        public string Ulice
        {
            get => _zamestnanec.Ulice;
            set
            {
                _zamestnanec.Ulice = value;
                OnPropertyChanged();
            }
        }

        public string CisloBudovy
        {
            get => _zamestnanec.CisloBudovy;
            set
            {
                _zamestnanec.CisloBudovy = value;
                OnPropertyChanged();
            }
        }

        public string ZipCode
        {
            get => _zamestnanec.ZipCode;
            set
            {
                _zamestnanec.ZipCode = value;
                OnPropertyChanged();
            }
        }

        public string CisloBytu
        {
            get => _zamestnanec.CisloBytu;
            set
            {
                _zamestnanec.CisloBytu = value;
                OnPropertyChanged();
            }
        }
    }
}
