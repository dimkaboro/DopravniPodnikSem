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
        private string _typ;

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

        public string Typ
        {
            get => _typ;
            set => SetField(ref _typ, value);
        }

        // Навигационное свойство для связи с Jizda
        public virtual ICollection<Jizda> Jizdy { get; set; } = new List<Jizda>();
    }
}

