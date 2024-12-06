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
        private string _registracniCislo;
        private string _typ;
        private int? _kapacita;
        private int? _garazeGarazId;
        private int? _udrzbaVozidlaUdrzbaId;

        public int VozidloId
        {
            get => _vozidloId;
            set => SetField(ref _vozidloId, value);
        }

        public string RegistracniCislo
        {
            get => _registracniCislo;
            set => SetField(ref _registracniCislo, value);
        }

        public string Typ
        {
            get => _typ;
            set => SetField(ref _typ, value);
        }

        public int? Kapacita
        {
            get => _kapacita;
            set => SetField(ref _kapacita, value);
        }

        public int? GarazeGarazId
        {
            get => _garazeGarazId;
            set => SetField(ref _garazeGarazId, value);
        }

        public int? UdrzbaVozidlaUdrzbaId
        {
            get => _udrzbaVozidlaUdrzbaId;
            set => SetField(ref _udrzbaVozidlaUdrzbaId, value);
        }

        // Навигационные свойства
        public virtual Garaz Garaz { get; set; }             // Связь с Garaz
        public virtual UdrzbaVozidla UdrzbaVozidla { get; set; } // Связь с UdrzbaVozidla
    }
}

