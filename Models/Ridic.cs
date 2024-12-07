using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Ridic : BaseModel
    {
        private int _ridicId;
        private string _jmeno;
        private string _prijmeni;
        private string _ridicPrukaz;
        private DateOnly? _datumNarozeni; // Сделано nullable, так как поле необязательное

        public string RidicName => $"{_jmeno} {_prijmeni}";

        public override string ToString()
        {
            return RidicName;
        }

        public int RidicId
        {
            get => _ridicId;
            set => SetField(ref _ridicId, value);
        }

        public string Jmeno
        {
            get => _jmeno;
            set => SetField(ref _jmeno, value);
        }

        public string Prijmeni
        {
            get => _prijmeni;
            set => SetField(ref _prijmeni, value);
        }

        public string RidicPrukaz
        {
            get => _ridicPrukaz;
            set => SetField(ref _ridicPrukaz, value);
        }

        public DateOnly? DatumNarozeni
        {
            get => _datumNarozeni;
            set => SetField(ref _datumNarozeni, value);
        }

        // Навигационные свойства для Jizdy
        public virtual ICollection<Jizda> Jizdy { get; set; } = new List<Jizda>();
    }
}
