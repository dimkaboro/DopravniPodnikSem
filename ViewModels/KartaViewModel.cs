using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class KartaViewModel : BaseViewModel
    {
        private Karta _karta;

        public KartaViewModel(Karta karta)
        {
            _karta = karta;
        }

        public int CisloUctu
        {
            get => _karta.CisloUctu;
            set
            {
                _karta.CisloUctu = value;
                OnPropertyChanged();
            }
        }

        public int CisloKarty
        {
            get => _karta.CisloKarty;
            set
            {
                _karta.CisloKarty = value;
                OnPropertyChanged();
            }
        }
    }
}
