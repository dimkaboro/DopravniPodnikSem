using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class RidiciNaLinceViewModel : BaseViewModel
    {
        private RidiciNaLince _ridiciNaLince;

        public RidiciNaLinceViewModel(RidiciNaLince ridiciNaLince)
        {
            _ridiciNaLince = ridiciNaLince;
        }

        public int RidicId
        {
            get => _ridiciNaLince.RidicId;
            set
            {
                _ridiciNaLince.RidicId = value;
                OnPropertyChanged();
            }
        }

        public int LinkaId
        {
            get => _ridiciNaLince.LinkaId;
            set
            {
                _ridiciNaLince.LinkaId = value;
                OnPropertyChanged();
            }
        }
    }
}
