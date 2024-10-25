using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class VozidloDopravniPlatbaViewModel : BaseViewModel
    {
        private VozidloDopravniPlatba _vozidloDopravniPlatba;

        public VozidloDopravniPlatbaViewModel(VozidloDopravniPlatba vozidloDopravniPlatba)
        {
            _vozidloDopravniPlatba = vozidloDopravniPlatba;
        }

        public int BiletId
        {
            get => _vozidloDopravniPlatba.BiletId;
            set
            {
                _vozidloDopravniPlatba.BiletId = value;
                OnPropertyChanged();
            }
        }

        public int VozidloId
        {
            get => _vozidloDopravniPlatba.VozidloId;
            set
            {
                _vozidloDopravniPlatba.VozidloId = value;
                OnPropertyChanged();
            }
        }

        public DopravniPlatba DopravniPlatba
        {
            get => _vozidloDopravniPlatba.DopravniPlatba;
            set
            {
                _vozidloDopravniPlatba.DopravniPlatba = value;
                OnPropertyChanged();
            }
        }

        public Vozidlo Vozidlo
        {
            get => _vozidloDopravniPlatba.Vozidlo;
            set
            {
                _vozidloDopravniPlatba.Vozidlo = value;
                OnPropertyChanged();
            }
        }
    }
}
