using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Garaz : BaseModel
    {
        private int _garazId;
        private string _nazev;
        private string _adresa;
        private int _kapacita;

        public int GarazId
        {
            get => _garazId;
            set => SetField(ref _garazId, value);
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

        public int Kapacita
        {
            get => _kapacita;
            set => SetField(ref _kapacita, value);
        }
    }
}

