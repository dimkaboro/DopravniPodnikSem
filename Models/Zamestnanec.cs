using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Zamestnanec : BaseModel
    {
        private int _zamestnanecId;
        private string _jmeno;
        private string _prijmeni;
        private string _pozice;
        private decimal _plat;
        private DateTime _datumNastupu;
        private int? _zamestnanecZamestnanecId;
        private string _email;
        private string _heslo;
        private string _cisloTelefonu;
        private string _mesto;
        private string _ulice;
        private string _cisloBudovy;
        private string _zipCode;
        private string _cisloBytu;

        public int ZamestnanecId
        {
            get => _zamestnanecId;
            set => SetField(ref _zamestnanecId, value);
        }

        public string Jmeno
        {
            get => _jmeno;
            set => SetField(ref _jmeno, value);
        }

        public string Prijmeni
        {
            get => _prijmeni;
            set => SetField(ref _prijmeni, value);
        }

        public string Pozice
        {
            get => _pozice;
            set => SetField(ref _pozice, value);
        }

        public decimal Plat
        {
            get => _plat;
            set => SetField(ref _plat, value);
        }

        public DateTime DatumNastupu
        {
            get => _datumNastupu;
            set => SetField(ref _datumNastupu, value);
        }

        public int? ZamestnanecZamestnanecId
        {
            get => _zamestnanecZamestnanecId;
            set => SetField(ref _zamestnanecZamestnanecId, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string Heslo
        {
            get => _heslo;
            set => SetField(ref _heslo, value);
        }

        public string CisloTelefonu
        {
            get => _cisloTelefonu;
            set => SetField(ref _cisloTelefonu, value);
        }

        public string Mesto
        {
            get => _mesto;
            set => SetField(ref _mesto, value);
        }

        public string Ulice
        {
            get => _ulice;
            set => SetField(ref _ulice, value);
        }

        public string CisloBudovy
        {
            get => _cisloBudovy;
            set => SetField(ref _cisloBudovy, value);
        }

        public string ZipCode
        {
            get => _zipCode;
            set => SetField(ref _zipCode, value);
        }

        public string CisloBytu
        {
            get => _cisloBytu;
            set => SetField(ref _cisloBytu, value);
        }

        // Навигационные свойства
        public virtual ICollection<Garaz> Garaze { get; set; } // Связанные Garaz
        public virtual Zamestnanec Vedouci { get; set; } // Связь с руководителем
        public virtual ICollection<Zamestnanec> Podrizeni { get; set; } // Связь с подчинёнными
    }
}

