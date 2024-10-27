using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkaTrasaViewModel : BaseViewModel
    {
        private ZastavkaTrasa _zastavkaTrasa;

        public ZastavkaTrasaViewModel(ZastavkaTrasa zastavkaTrasa)
        {
            _zastavkaTrasa = zastavkaTrasa;
        }

        public int ZastavkaTrasaId
        {
            get => _zastavkaTrasa.ZastavkaTrasaId;
            set
            {
                _zastavkaTrasa.ZastavkaTrasaId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasPrijezdu
        {
            get => _zastavkaTrasa.CasPrijezdu;
            set
            {
                _zastavkaTrasa.CasPrijezdu = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasOdjezdu
        {
            get => _zastavkaTrasa.CasOdjezdu;
            set
            {
                _zastavkaTrasa.CasOdjezdu = value;
                OnPropertyChanged();
            }
        }
    }
}
