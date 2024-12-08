using DopravniPodnikSem.Models.Enum;
using System;
using System.Collections.Generic;

namespace DopravniPodnikSem.Models
{
    public class Zamestnanec : BaseModel
    {
        private int _zamestnanecId;
        private string _jmeno;
        private string _prijmeni;
        private string _pozice;
        private int _plat;
        private DateOnly _datumNastupu;
        private int? _zamestnanecZamestnanecId;
        private string _email;
        private string _heslo;
        private string _cisloTelefonu;
        private int _adresaId; 
        private int _roleId; 
        private int _souborId; 
        private int _jePrivate;

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

        public int Plat
        {
            get => _plat;
            set => SetField(ref _plat, value);
        }

        public DateOnly DatumNastupu
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

        public int AdresaId
        {
            get => _adresaId;
            set => SetField(ref _adresaId, value);
        }

        public int RoleId
        {
            get => _roleId;
            set => SetField(ref _roleId, value);
        }

        public Role Role
        {
            get => (Role)RoleId; 
            set => RoleId = (int)value; 
        }

        public int SouborId
        {
            get => _souborId;
            set => SetField(ref _souborId, value);
        }

        public int JePrivate
        {
            get => _jePrivate;
            set => SetField(ref _jePrivate, value);
        }

        public string FullName => $"{Jmeno} {Prijmeni}";
        public string FullNameAndPozice => $"{Jmeno} {Prijmeni} ({Pozice})";

        public virtual Adresa Adresa { get; set; } 
        public virtual Soubory Soubor { get; set; } 
        public virtual Zamestnanec Vedouci { get; set; } 
        public virtual ICollection<Zamestnanec> Podrizeni { get; set; } = new List<Zamestnanec>(); 
    }
}
