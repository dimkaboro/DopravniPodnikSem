using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.Models
{
    public class VozidloDopravniPlatba : BaseModel
    {
        private int _biletId;
        private int _vozidloId;

        public int BiletId
        {
            get => _biletId;
            set => SetField(ref _biletId, value);
        }

        public int VozidloId
        {
            get => _vozidloId;
            set => SetField(ref _vozidloId, value);
        }

        public virtual DopravniPlatba DopravniPlatba { get; set; }
        public virtual Vozidlo Vozidlo { get; set; }
    }
}
