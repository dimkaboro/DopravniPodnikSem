using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class TypLinky : BaseModel
    {
        private int _typLinkyId;
        private string _typ;

        public int TypLinkyId
        {
            get => _typLinkyId;
            set => SetField(ref _typLinkyId, value);
        }

        public string Typ
        {
            get => _typ;
            set => SetField(ref _typ, value);
        }
    }
}
