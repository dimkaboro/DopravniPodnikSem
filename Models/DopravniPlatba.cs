using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class DopravniPlatba : BaseModel
    {
        private int _biletId;
        private decimal _cena;
        private DateTime _datumnakupu;
        private string _typPlatby;

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
            get => _datumnakupu;
            set => SetField(ref _datumnakupu, value);
        }

        public string TypPlatby
        {
            get => _typPlatby;
            set => SetField(ref _typPlatby, value);
        }
    }
}
