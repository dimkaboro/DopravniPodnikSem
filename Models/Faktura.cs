using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Faktura : DopravniPlatba
    {
        private int _cislouctu;
        private DateTime _datumsplatnosti;

        public int CisloUctu
        {
            get => _cislouctu;
            set => SetField(ref _cislouctu, value);
        }

        public DateTime DatumSplatnosti
        {
            get => _datumsplatnosti;
            set => SetField(ref _datumsplatnosti, value);
        }
    }
}
