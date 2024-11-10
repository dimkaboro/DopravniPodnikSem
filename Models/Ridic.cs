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
        private DateTime _datumNarozeni;
        private int _vozidloVozidloId;
        private int _jizdaJizdaId;

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

        public DateTime DatumNarozeni
        {
            get => _datumNarozeni;
            set => SetField(ref _datumNarozeni, value);
        }

        public int VozidloVozidloId
        {
            get => _vozidloVozidloId;
            set => SetField(ref _vozidloVozidloId, value);
        }

        public int JizdaJizdaId
        {
            get => _jizdaJizdaId;
            set => SetField(ref _jizdaJizdaId, value);
        }

        // Навигационные свойства
        public virtual Vozidlo Vozidlo { get; set; } // Связь с Vozidlo
        public virtual Jizda Jizda { get; set; }     // Связь с Jizda
    }
}
