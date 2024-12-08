using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.Models
{
    public class DopravniPlatba : BaseModel
    {
        private int _biletId;
        private decimal _cena;
        private DateTime _datumNakupu;
        private string _typPlatby;
        private int _jizdaJizdaId;

        public int BiletId
        {
            get => _biletId;
            set => SetField(ref _biletId, value);
        }

        public decimal Cena
        {
            get => _cena;
            set => SetField(ref _cena, value);
        }

        public DateTime DatumNakupu
        {
            get => _datumNakupu;
            set => SetField(ref _datumNakupu, value);
        }

        public string TypPlatby
        {
            get => _typPlatby;
            set => SetField(ref _typPlatby, value);
        }

        public int JizdaJizdaId
        {
            get => _jizdaJizdaId;
            set => SetField(ref _jizdaJizdaId, value);
        }

        public virtual Jizda Jizda { get; set; } 
    }
}
