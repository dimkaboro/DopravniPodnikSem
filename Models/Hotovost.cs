using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Hotovost : DopravniPlatba
    {
        private int _biletId;
        private decimal _prijato;
        private decimal _vraceno;

        public int BiletId
        {
            get => _biletId;
            set => SetField(ref _biletId, value);
        }

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

        public virtual DopravniPlatba DopravniPlatba { get; set; }
    }
}
