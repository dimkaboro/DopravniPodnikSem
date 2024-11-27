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
        private string _popis;

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

        public string Popis
        {
            get => _popis;
            set => SetField(ref _popis, value);
        }
    }
}

