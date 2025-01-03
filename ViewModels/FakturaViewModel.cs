﻿using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class FakturaViewModel : BaseViewModel
    {
        private Faktura _faktura;

        public FakturaViewModel(Faktura faktura)
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


        public int BiletId
        {
            get => _faktura.BiletId;
            set
            {
                _faktura.BiletId = value;
                OnPropertyChanged();
            }
        }

        public decimal Cena
        {
            get => _faktura.Cena;
            set
            {
                _faktura.Cena = value;
                OnPropertyChanged();
            }
        }
    }
}
