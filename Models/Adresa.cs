using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Adresa : BaseModel
    {
        private int _adresaId;
        private string _mesto;
        private string _ulice;
        private string _cisloBudovy;
        private string _zipCode;
        private string _cisloBytu;

        public int AdresaId
        {
            get => _adresaId;
            set => SetField(ref _adresaId, value);
        }

        public string Mesto
        {
            get => _mesto;
            set => SetField(ref _mesto, value);
        }

        public string Ulice
        {
            get => _ulice;
            set => SetField(ref _ulice, value);
        }

        public string CisloBudovy
        {
            get => _cisloBudovy;
            set => SetField(ref _cisloBudovy, value);
        }

        public string ZipCode
        {
            get => _zipCode;
            set => SetField(ref _zipCode, value);
        }

        public string CisloBytu
        {
            get => _cisloBytu;
            set => SetField(ref _cisloBytu, value);
        }

        // Навигационные свойства
        public virtual ICollection<Zamestnanec> Zamestnanci { get; set; } = new List<Zamestnanec>();
    }
}
