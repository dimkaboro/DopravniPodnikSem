using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Soubory : BaseModel
    {
        private int _souborId;
        private string _nazev;
        private byte[] _soubor;
        private string _typSouboru;
        private string _priponaSouboru;
        private DateOnly _datumNahrani;
        private DateOnly _datumModifikace;
        private string _operaceProvedl;

        public int SouborId
        {
            get => _souborId;
            set => SetField(ref _souborId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public byte[] Soubor
        {
            get => _soubor;
            set => SetField(ref _soubor, value);
        }

        public string TypSouboru
        {
            get => _typSouboru;
            set => SetField(ref _typSouboru, value);
        }

        public string PriponaSouboru
        {
            get => _priponaSouboru;
            set => SetField(ref _priponaSouboru, value);
        }

        public DateOnly DatumNahrani
        {
            get => _datumNahrani;
            set => SetField(ref _datumNahrani, value);
        }

        public DateOnly DatumModifikace
        {
            get => _datumModifikace;
            set => SetField(ref _datumModifikace, value);
        }

        public string OperaceProvedl
        {
            get => _operaceProvedl;
            set => SetField(ref _operaceProvedl, value);
        }

        public virtual ICollection<Zamestnanec> Zamestnanci { get; set; } = new List<Zamestnanec>();
    }
}
