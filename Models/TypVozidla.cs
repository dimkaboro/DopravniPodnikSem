using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class TypVozidla : BaseModel
    {
        private int _typVozidlaId;
        private string _typ;

        public int TypVozidlaId
        {
            get => _typVozidlaId;
            set => SetField(ref _typVozidlaId, value);
        }

        public string Typ
        {
            get => _typ;
            set => SetField(ref _typ, value);
        }
    }
}
