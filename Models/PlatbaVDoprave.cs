using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class PlatbaVDoprave : BaseModel
    {
        private int _platbaId;
        private decimal _cena;
        private DateTime _datumPlatby;
        private string _typPlatby;

        public int PlatbaId
        {
            get => _platbaId;
            set => SetField(ref _platbaId, value);
        }

        public decimal Cena
        {
            get => _cena;
            set => SetField(ref _cena, value);
        }

        public DateTime DatumPlatby
        {
            get => _datumPlatby;
            set => SetField(ref _datumPlatby, value);
        }

        public string TypPlatby
        {
            get => _typPlatby;
            set => SetField(ref _typPlatby, value);
        }
    }
}

