using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Zastavka : BaseModel
    {
        private int _zastavkaId;
        private string _nazev;
        private string _adresa;

        public int ZastavkaId
        {
            get => _zastavkaId;
            set => SetField(ref _zastavkaId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public string Adresa
        {
            get => _adresa;
            set => SetField(ref _adresa, value);
        }
    }
}

