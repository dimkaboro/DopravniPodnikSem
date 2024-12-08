using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Jizda : BaseModel
    {
        private int _jizdaId;
        private DateTime _casOd;
        private DateTime _casDo;
        private string _stav;
        private int _linkaId;
        private int _ridicId;
        private int _vozidloId;

        public int JizdaId
        {
            get => _jizdaId;
            set => SetField(ref _jizdaId, value);
        }

        public DateTime CasOd
        {
            get => _casOd;
            set => SetField(ref _casOd, value);
        }

        public DateTime CasDo
        {
            get => _casDo;
            set => SetField(ref _casDo, value);
        }

        public string Stav
        {
            get => _stav;
            set => SetField(ref _stav, value);
        }

        public int LinkaId
        {
            get => _linkaId;
            set => SetField(ref _linkaId, value);
        }

        public int RidicId
        {
            get => _ridicId;
            set => SetField(ref _ridicId, value);
        }

        public int VozidloId
        {
            get => _vozidloId;
            set => SetField(ref _vozidloId, value);
        }

        public virtual Linka Linka { get; set; }       
        public virtual Ridic Ridic { get; set; }       
        public virtual Vozidlo Vozidlo { get; set; }   
    }
}

