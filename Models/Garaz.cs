using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Garaz : BaseModel
    {
        private int _garazId;
        private string _nazev;
        private string _adresa;
        private int _zamestnanecId;

        public int GarazId
        {
            get => _garazId;
            set => SetField(ref _garazId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public string Adresa
        {
            get => _adresa;
            set => SetField(ref _adresa, value);
        }

        public int ZamestnanecId
        {
            get => _zamestnanecId;
            set => SetField(ref _zamestnanecId, value);
        }

        public virtual Zamestnanec Zamestnanec { get; set; }
        public virtual ICollection<Vozidlo> Vozidla { get; set; }
    }
}

