using DopravniPodnikSem.Models;
using DopravniPodnikSem.Models.Enum;

namespace DopravniPodnikSem.ViewModels
{
    public class ZamestnanecViewModel : BaseViewModel
    {
        private readonly Zamestnanec _zamestnanec;

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

        public int Plat
        {
            get => _zamestnanec.Plat;
            set
            {
                _zamestnanec.Plat = value;
                OnPropertyChanged();
            }
        }

        public DateOnly DatumNastupu
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

        public int AdresaId
        {
            get => _zamestnanec.AdresaId;
            set
            {
                _zamestnanec.AdresaId = value;
                OnPropertyChanged();
            }
        }

        public int RoleId
        {
            get => _zamestnanec.RoleId;
            set
            {
                _zamestnanec.RoleId = value;
                OnPropertyChanged();
            }
        }

        public int SouborId
        {
            get => _zamestnanec.SouborId;
            set
            {
                _zamestnanec.SouborId = value;
                OnPropertyChanged();
            }
        }

        public int JePrivate
        {
            get => _zamestnanec.JePrivate;
            set
            {
                _zamestnanec.JePrivate = value;
                OnPropertyChanged();
            }
        }

        // Навигационные свойства
        public AdresaViewModel AdresaViewModel => new AdresaViewModel(_zamestnanec.Adresa);
        public SouboryViewModel SouborViewModel => new SouboryViewModel(_zamestnanec.Soubor);

        public ZamestnanecViewModel VedouciViewModel => _zamestnanec.Vedouci != null ? new ZamestnanecViewModel(_zamestnanec.Vedouci) : null;

        public IEnumerable<ZamestnanecViewModel> PodrizeniViewModel => _zamestnanec.Podrizeni?.Select(z => new ZamestnanecViewModel(z));

        // Свойство для Role (из Enum)
        public Role Role
        {
            get => (Role)_zamestnanec.RoleId;
            set
            {
                _zamestnanec.RoleId = (int)value;
                OnPropertyChanged();
            }
        }
    }
}
