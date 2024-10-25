using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Hotovost : DopravniPlatba
    {
        private decimal _prijato;
        private decimal _vraceno;

        public decimal Prijato
        {
            get => _prijato;
            set => SetField(ref _prijato, value);
        }

        public decimal Vraceno
        {
            get => _vraceno;
            set => SetField(ref _vraceno, value);
        }
    }
}
