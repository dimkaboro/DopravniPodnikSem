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

        public string TypUdrzby
        {
            get => _udrzbaVozidla.TypUdrzby;
            set
            {
                _udrzbaVozidla.TypUdrzby = value;
                OnPropertyChanged();
            }
        }

        public decimal Naklady
        {
            get => _udrzbaVozidla.Naklady;
            set
            {
                _udrzbaVozidla.Naklady = value;
                OnPropertyChanged();
            }
        }
    }
}
