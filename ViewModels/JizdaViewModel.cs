using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class JizdaViewModel : BaseViewModel
    {
        private Jizda _jizda;

        public JizdaViewModel(Jizda jizda)
        {
            _jizda = jizda;
        }

        public int JizdaId
        {
            get => _jizda.JizdaId;
            set
            {
                _jizda.JizdaId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasStart
        {
            get => _jizda.CasStart;
            set
            {
                _jizda.CasStart = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasEnd
        {
            get => _jizda.CasEnd;
            set
            {
                _jizda.CasEnd = value;
                OnPropertyChanged();
            }
        }

        public string Stav
        {
            get => _jizda.Stav;
            set
            {
                _jizda.Stav = value;
                OnPropertyChanged();
            }
        }

        public int LinkaLinkaId
        {
            get => _jizda.LinkaLinkaId;
            set
            {
                _jizda.LinkaLinkaId = value;
                OnPropertyChanged();
            }
        }
    }
}
