using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class UdrzbaVozidlaViewModel : BaseViewModel
    {
        private UdrzbaVozidla _udrzbaVozidla;

        public UdrzbaVozidlaViewModel(UdrzbaVozidla udrzbaVozidla)
        {
            _udrzbaVozidla = udrzbaVozidla;
        }

        public int UdrzbaId
        {
            get => _udrzbaVozidla.UdrzbaId;
            set
            {
                _udrzbaVozidla.UdrzbaId = value;
                OnPropertyChanged();
            }
        }

        public DateTime DatumUdrzby
        {
            get => _udrzbaVozidla.DatumUdrzby;
            set
            {
                _udrzbaVozidla.DatumUdrzby = value;
                OnPropertyChanged();
            }
        }
    }
}
