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
        private int? _typId;
        private string _typNazev; 
        private int? _kapacita;
        private int? _garazeGarazId;
        private int? _udrzbaVozidlaUdrzbaId;

        public string VehicleInfo => $"{RegistracniCislo} - {TypNazev}";

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

        public int? TypId
        {
            get => _typId;
            set => SetField(ref _typId, value);
        }

        public string TypNazev
        {
            get => _typNazev;
            set => SetField(ref _typNazev, value);
        }

        public virtual TypVozidla TypVozidla { get; set; }

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

        public virtual Garaz Garaz { get; set; }
        public virtual UdrzbaVozidla UdrzbaVozidla { get; set; }
    }
}

