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
        private int? _zamestnanecZamestnanecId; // Самоссылка на ID другого Zamestnanec

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

        // Навигационные свойства
        public virtual ICollection<Garaz> Garaze { get; set; } // Связанные Garaz
        public virtual Zamestnanec Vedouci { get; set; } // Связь с руководителем
        public virtual ICollection<Zamestnanec> Podrizeni { get; set; } // Связь с подчинёнными
    }
}

