using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class LinkaViewModel : BaseViewModel
    {
        private Linka _linka;

        public LinkaViewModel(Linka linka)
        {
            _linka = linka;
        }

        public int LinkaId
        {
            get => _linka.LinkaId;
            set
            {
                _linka.LinkaId = value;
                OnPropertyChanged();
            }
        }

        public string Nazev
        {
            get => _linka.Nazev;
            set
            {
                _linka.Nazev = value;
                OnPropertyChanged();
            }
        }

        public string Popis
        {
            get => _linka.Popis;
            set
            {
                _linka.Popis = value;
                OnPropertyChanged();
            }
        }

        public string Typ
        {
            get => _linka.Typ;
            set
            {
                _linka.Typ = value;
                OnPropertyChanged();
            }
        }
    }
}
