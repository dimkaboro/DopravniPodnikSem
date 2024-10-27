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
        private int _kapacita;
        private int _garazId;
        private int _udrzbaVozidelId;

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

        public int Kapacita
        {
            get => _kapacita;
            set => SetField(ref _kapacita, value);
        }

        public int GarazId
        {
            get => _garazId;
            set => SetField(ref _garazId, value);
        }

        public int UdrzbaVozidelId
        {
            get => _udrzbaVozidelId;
            set => SetField(ref _udrzbaVozidelId, value);
        }

        // Навигационные свойства для связей с Garaz и UdrzbaVozidla
        public virtual Garaz Garaz { get; set; }
        public virtual UdrzbaVozidla UdrzbaVozidla { get; set; }
    }
}

