using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.ViewModels
{
    public class JizdniRadViewModel : BaseViewModel
    {
        private JizdniRad _jizdniRad;

        public JizdniRadViewModel(JizdniRad jizdniRad)
        {
            _jizdniRad = jizdniRad;
        }

        public int JizdniRadId
        {
            get => _jizdniRad.JizdniRadId;
            set
            {
                _jizdniRad.JizdniRadId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasOdjezdu
        {
            get => _jizdniRad.CasOdjezdu;
            set
            {
                _jizdniRad.CasOdjezdu = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasPrijezdu
        {
            get => _jizdniRad.CasPrijezdu;
            set
            {
                _jizdniRad.CasPrijezdu = value;
                OnPropertyChanged();
            }
        }

        public string Den
        {
            get => _jizdniRad.Den;
            set
            {
                _jizdniRad.Den = value;
                OnPropertyChanged();
            }
        }
    }
}
