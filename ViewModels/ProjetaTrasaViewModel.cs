using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class ProjetaTrasaViewModel : BaseViewModel
    {
        private ProjetaTrasa _projetaTrasa;

        public ProjetaTrasaViewModel(ProjetaTrasa projetaTrasa)
        {
            _projetaTrasa = projetaTrasa;
        }

        public int ProjetaTrasaId
        {
            get => _projetaTrasa.ProjetaTrasaId;
            set
            {
                _projetaTrasa.ProjetaTrasaId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasStart
        {
            get => _projetaTrasa.CasStart;
            set
            {
                _projetaTrasa.CasStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasEnd
        {
            get => _projetaTrasa.CasEnd;
            set
            {
                _projetaTrasa.CasEnd = value;
                OnPropertyChanged();
            }
        }
    }
}
