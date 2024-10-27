using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class LinkaProjetaTrasaViewModel : BaseViewModel
    {
        private LinkaProjetaTrasa _linkaProjetaTrasa;

        public LinkaProjetaTrasaViewModel(LinkaProjetaTrasa linkaProjetaTrasa)
        {
            _linkaProjetaTrasa = linkaProjetaTrasa;
        }

        public int LinkaId
        {
            get => _linkaProjetaTrasa.LinkaId;
            set
            {
                _linkaProjetaTrasa.LinkaId = value;
                OnPropertyChanged();
            }
        }

        public int ProjetaTrasaId
        {
            get => _linkaProjetaTrasa.ProjetaTrasaId;
            set
            {
                _linkaProjetaTrasa.ProjetaTrasaId = value;
                OnPropertyChanged();
            }
        }

        public Linka Linka
        {
            get => _linkaProjetaTrasa.Linka;
            set
            {
                _linkaProjetaTrasa.Linka = value;
                OnPropertyChanged();
            }
        }

        public ProjetaTrasa ProjetaTrasa
        {
            get => _linkaProjetaTrasa.ProjetaTrasa;
            set
            {
                _linkaProjetaTrasa.ProjetaTrasa = value;
                OnPropertyChanged();
            }
        }
    }
}
