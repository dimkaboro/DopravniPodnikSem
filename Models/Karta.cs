using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Karta : DopravniPlatba
    {
        private int _biletId;
        private int _cisloUctu;
        private int _cisloKarty;

        public int BiletId
        {
            get => _biletId;
            set => SetField(ref _biletId, value);
        }

        public int CisloUctu
        {
            get => _cisloUctu;
            set => SetField(ref _cisloUctu, value);
        }

        public int CisloKarty
        {
            get => _cisloKarty;
            set => SetField(ref _cisloKarty, value);
        }

        public virtual DopravniPlatba DopravniPlatba { get; set; }
    }
}
