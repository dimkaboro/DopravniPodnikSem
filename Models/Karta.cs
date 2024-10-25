using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Karta : DopravniPlatba
    {
        private int _cislouctu;
        private int _cislokarty;

        public int CisloUctu
        {
            get => _cislouctu;
            set => SetField(ref _cislouctu, value);
        }

        public int CisloKarty
        {
            get => _cislokarty;
            set => SetField(ref _cislokarty, value);
        }
    }
}
