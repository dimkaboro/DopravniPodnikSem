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

        public virtual ICollection<Zamestnanec> Zamestnanci { get; set; } = new List<Zamestnanec>();
    }
}
