using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class GarazViewModel : BaseViewModel
    {
        private Garaz _garaz;

        public GarazViewModel(Garaz garaz)
        {
            _garaz = garaz;
        }

        public int GarazId
        {
            get => _garaz.GarazId;
            set
            {
                _garaz.GarazId = value;
                OnPropertyChanged();
            }
        }

        public string Nazev
        {
            get => _garaz.Nazev;
            set
            {
                _garaz.Nazev = value;
                OnPropertyChanged();
            }
        }

        public string Adresa
        {
            get => _garaz.Adresa;
            set
            {
                _garaz.Adresa = value;
                OnPropertyChanged();
            }
        }
    }
}
