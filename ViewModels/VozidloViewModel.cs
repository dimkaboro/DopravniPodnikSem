using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class VozidloViewModel : BaseViewModel
    {
        private Vozidlo _vozidlo;

        public VozidloViewModel(Vozidlo vozidlo)
        {
            _vozidlo = vozidlo;
        }

        public int VozidloId
        {
            get => _vozidlo.VozidloId;
            set
            {
                _vozidlo.VozidloId = value;
                OnPropertyChanged();
            }
        }

        public string RegistracniCislo
        {
            get => _vozidlo.RegistracniCislo;
            set
            {
                _vozidlo.RegistracniCislo = value;
                OnPropertyChanged();
            }
        }

        public string Typ
        {
            get => _vozidlo.Typ;
            set
            {
                _vozidlo.Typ = value;
                OnPropertyChanged();
            }
        }

        public int Kapacita
        {
            get => _vozidlo.Kapacita;
            set
            {
                _vozidlo.Kapacita = value;
                OnPropertyChanged();
            }
        }
    }
}
