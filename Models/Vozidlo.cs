using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Vozidlo : BaseModel
    {
        private int _vozidloId;
        private string _spz;
        private string _typ;
        private DateTime _datumVyroby;
        private int _garazId;

        public int VozidloId
        {
            get => _vozidloId;
            set => SetField(ref _vozidloId, value);
        }

        public string SPZ
        {
            get => _spz;
            set => SetField(ref _spz, value);
        }

        public string Typ
        {
            get => _typ;
            set => SetField(ref _typ, value);
        }

        public DateTime DatumVyroby
        {
            get => _datumVyroby;
            set => SetField(ref _datumVyroby, value);
        }

        public int GarazId
        {
            get => _garazId;
            set => SetField(ref _garazId, value);
        }
    }
}

