using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class FakturaViewModel : DopravniPlatbaViewModel
    {
        private readonly Faktura _faktura;

        public FakturaViewModel(Faktura faktura) : base(faktura)
        {
            _faktura = faktura;
        }

        public int CisloUctu
        {
            get => _faktura.CisloUctu;
            set
            {
                _faktura.CisloUctu = value;
                OnPropertyChanged();
            }
        }

        public DateTime DatumSplatnosti
        {
            get => _faktura.DatumSplatnosti;
            set
            {
                _faktura.DatumSplatnosti = value;
                OnPropertyChanged();
            }
        }
    }
}
