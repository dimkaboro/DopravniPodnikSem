using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class RidiciNaLince : BaseModel
    {
        private int _ridicId;
        private int _linkaId;

        public int RidicId
        {
            get => _ridicId;
            set => SetField(ref _ridicId, value);
        }

        public int LinkaId
        {
            get => _linkaId;
            set => SetField(ref _linkaId, value);
        }
    }
}
