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
        private int _zastavkaId;

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

        public int ZastavkaId
        {
            get => _zastavkaId;
            set => SetField(ref _zastavkaId, value);
        }

        // Навигационное свойство для связи с Zastavka
        public virtual Zastavka Zastavka { get; set; }

        // Навигационное свойство для коллекции Zastavky, если необходимо для других целей
        public virtual ICollection<Zastavka> Zastavky { get; set; }
    }
}

