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
            get => _jizda.CasOd;
            set
            {
                _jizda.CasOd = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasEnd
        {
            get => _jizda.CasDo;
            set
            {
                _jizda.CasDo = value;
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

        public int LinkaId
        {
            get => _jizda.LinkaId;
            set
            {
                _jizda.LinkaId = value;
                OnPropertyChanged();
            }
        }

        // Навигационные свойства
        public virtual Linka Linka => _jizda.Linka;  // Связь с Linka
        public virtual Ridic Ridic => _jizda.Ridic;  // Связь с Ridic
        public virtual Vozidlo Vozidlo => _jizda.Vozidlo; // Связь с Vozidlo
    }
}
