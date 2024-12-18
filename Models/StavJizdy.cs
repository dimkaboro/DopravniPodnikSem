using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class StavJizdy : BaseModel
    {
        private int _stavJizdyId;
        private string _stav;

        public int StavJizdyId
        {
            get => _stavJizdyId;
            set => SetField(ref _stavJizdyId, value);
        }

        public string Stav
        {
            get => _stav;
            set => SetField(ref _stav, value);
        }
    }
}
