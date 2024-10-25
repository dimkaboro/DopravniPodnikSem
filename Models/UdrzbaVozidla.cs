using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class UdrzbaVozidla : BaseModel
    {
        private int _udrzbaId;
        private DateTime _datumUdrzby;
        private string _typUdrzby;
        private int _vozidloId;
        private decimal _naklady;

        public int UdrzbaId
        {
            get => _udrzbaId;
            set => SetField(ref _udrzbaId, value);
        }

        public DateTime DatumUdrzby
        {
            get => _datumUdrzby;
            set => SetField(ref _datumUdrzby, value);
        }

        public string TypUdrzby
        {
            get => _typUdrzby;
            set => SetField(ref _typUdrzby, value);
        }

        public int VozidloId
        {
            get => _vozidloId;
            set => SetField(ref _vozidloId, value);
        }

        public decimal Naklady
        {
            get => _naklady;
            set => SetField(ref _naklady, value);
        }
    }
}

