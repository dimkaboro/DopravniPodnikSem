using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class HotovostViewModel : BaseViewModel
    {
        private Hotovost _hotovost;

        public HotovostViewModel(Hotovost hotovost)
        {
            _hotovost = hotovost;
        }

        public decimal Prijato
        {
            get => _hotovost.Prijato;
            set
            {
                _hotovost.Prijato = value;
                OnPropertyChanged();
            }
        }

        public decimal Vraceno
        {
            get => _hotovost.Vraceno;
            set
            {
                _hotovost.Vraceno = value;
                OnPropertyChanged();
            }
        }
    }
}
