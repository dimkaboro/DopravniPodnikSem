using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class DopravniPlatbaViewModel : BaseViewModel
    {
        protected readonly DopravniPlatba _dopravniPlatba;

        public DopravniPlatbaViewModel(DopravniPlatba dopravniPlatba)
        {
            _dopravniPlatba = dopravniPlatba ?? throw new ArgumentNullException(nameof(dopravniPlatba));
        }

        // Общие свойства для всех типов DopravniPlatba
        public int BiletId
        {
            get => _dopravniPlatba.BiletId;
            set
            {
                if (_dopravniPlatba.BiletId != value)
                {
                    _dopravniPlatba.BiletId = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal Cena
        {
            get => _dopravniPlatba.Cena;
            set
            {
                if (_dopravniPlatba.Cena != value)
                {
                    _dopravniPlatba.Cena = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DatumNakupu
        {
            get => _dopravniPlatba.DatumNakupu;
            set
            {
                if (_dopravniPlatba.DatumNakupu != value)
                {
                    _dopravniPlatba.DatumNakupu = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TypPlatby
        {
            get => _dopravniPlatba.TypPlatby;
            set
            {
                if (_dopravniPlatba.TypPlatby != value)
                {
                    _dopravniPlatba.TypPlatby = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
