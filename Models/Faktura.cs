using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Faktura : BaseModel
    {
        private int _biletId;
        private int _cisloUctu;
        private DateTime _datumSplatnosti;

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

        public DateTime DatumSplatnosti
        {
            get => _datumSplatnosti;
            set => SetField(ref _datumSplatnosti, value);
        }

        public virtual DopravniPlatba DopravniPlatba { get; set; }
    }
}
