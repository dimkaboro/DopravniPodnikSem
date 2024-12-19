using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Linka : BaseModel
    {
        private int _linkaId;
        private string _nazev;
        private int _typLinkyId; 
        private string _typNazev; 

        public override string ToString()
        {
            return Nazev;
        }

        public int LinkaId
        {
            get => _linkaId;
            set => SetField(ref _linkaId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public int TypLinkyId
        {
            get => _typLinkyId;
            set => SetField(ref _typLinkyId, value);
        }

        public string TypNazev
        {
            get => _typNazev;
            set => SetField(ref _typNazev, value);
        }

        public virtual ICollection<Jizda> Jizdy { get; set; } = new List<Jizda>();
    }
}

