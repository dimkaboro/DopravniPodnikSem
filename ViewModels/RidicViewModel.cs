using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class RidicViewModel : BaseViewModel
    {
        private Ridic _ridic;

        public RidicViewModel(Ridic ridic)
        {
            _ridic = ridic;
        }

        public int RidicId
        {
            get => _ridic.RidicId;
            set
            {
                _ridic.RidicId = value;
                OnPropertyChanged();
            }
        }

        public string Jmeno
        {
            get => _ridic.Jmeno;
            set
            {
                _ridic.Jmeno = value;
                OnPropertyChanged();
            }
        }

        public string Prijmeni
        {
            get => _ridic.Prijmeni;
            set
            {
                _ridic.Prijmeni = value;
                OnPropertyChanged();
            }
        }

        public string RidicPrukaz
        {
            get => _ridic.RidicPrukaz;
            set
            {
                _ridic.RidicPrukaz = value;
                OnPropertyChanged();
            }
        }

        public DateTime DatumNarozeni
        {
            get => _ridic.DatumNarozeni;
            set
            {
                _ridic.DatumNarozeni = value;
                OnPropertyChanged();
            }
        }
    }
}
