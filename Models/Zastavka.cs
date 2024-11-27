using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Zastavka : BaseModel
    {
        private int _zastavkaId;
        private string _nazev;
        private string _gpsSouradnice;

        public int ZastavkaId
        {
            get => _zastavkaId;
            set => SetField(ref _zastavkaId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public string GpsSouradnice
        {
            get => _gpsSouradnice;
            set => SetField(ref _gpsSouradnice, value);
        }
    }
}

