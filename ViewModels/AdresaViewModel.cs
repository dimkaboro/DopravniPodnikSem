using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class AdresaViewModel : BaseViewModel
    {
        private Adresa _adresa;

        public AdresaViewModel(Adresa adresa)
        {
            _adresa = adresa;
        }

        public int AdresaId
        {
            get => _adresa.AdresaId;
            set
            {
                _adresa.AdresaId = value;
                OnPropertyChanged();
            }
        }

        public string Mesto
        {
            get => _adresa.Mesto;
            set
            {
                _adresa.Mesto = value;
                OnPropertyChanged();
            }
        }

        public string Ulice
        {
            get => _adresa.Ulice;
            set
            {
                _adresa.Ulice = value;
                OnPropertyChanged();
            }
        }

        public string CisloBudovy
        {
            get => _adresa.CisloBudovy;
            set
            {
                _adresa.CisloBudovy = value;
                OnPropertyChanged();
            }
        }

        public string ZipCode
        {
            get => _adresa.ZipCode;
            set
            {
                _adresa.ZipCode = value;
                OnPropertyChanged();
            }
        }

        public string CisloBytu
        {
            get => _adresa.CisloBytu;
            set
            {
                _adresa.CisloBytu = value;
                OnPropertyChanged();
            }
        }
    }
}
