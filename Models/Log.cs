using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class Log : BaseModel
    {
        private int _logId;
        private string _jakaTabulka;
        private string _operace;
        private DateTime _casOperace;
        private string _uzivatel;
        private string _popis;

        public int LogId
        {
            get => _logId;
            set => SetField(ref _logId, value);
        }

        public string JakaTabulka
        {
            get => _jakaTabulka;
            set => SetField(ref _jakaTabulka, value);
        }

        public string Operace
        {
            get => _operace;
            set => SetField(ref _operace, value);
        }

        public DateTime CasOperace
        {
            get => _casOperace;
            set => SetField(ref _casOperace, value);
        }

        public string Uzivatel
        {
            get => _uzivatel;
            set => SetField(ref _uzivatel, value);
        }

        public string Popis
        {
            get => _popis;
            set => SetField(ref _popis, value);
        }
    }
}
