using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class LogViewModel : BaseViewModel
    {
        private Log _log;

        public LogViewModel(Log log)
        {
            _log = log;
        }

        public int LogId
        {
            get => _log.LogId;
            set
            {
                _log.LogId = value;
                OnPropertyChanged();
            }
        }

        public string JakaTabulka
        {
            get => _log.JakaTabulka;
            set
            {
                _log.JakaTabulka = value;
                OnPropertyChanged();
            }
        }

        public string Operace
        {
            get => _log.Operace;
            set
            {
                _log.Operace = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasOperace
        {
            get => _log.CasOperace;
            set
            {
                _log.CasOperace = value;
                OnPropertyChanged();
            }
        }

        public string Uzivatel
        {
            get => _log.Uzivatel;
            set
            {
                _log.Uzivatel = value;
                OnPropertyChanged();
            }
        }

        public string Popis
        {
            get => _log.Popis;
            set
            {
                _log.Popis = value;
                OnPropertyChanged();
            }
        }
    }
}
