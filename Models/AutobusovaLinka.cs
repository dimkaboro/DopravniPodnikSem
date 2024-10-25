using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class AutobusovaLinka : BaseModel
    {
        private int _autobusovaLinkaId;
        private string _nazev;
        private int _pocetZastavek;

        public int AutobusovaLinkaId
        {
            get => _autobusovaLinkaId;
            set => SetField(ref _autobusovaLinkaId, value);
        }

        public string Nazev
        {
            get => _nazev;
            set => SetField(ref _nazev, value);
        }

        public int PocetZastavek
        {
            get => _pocetZastavek;
            set => SetField(ref _pocetZastavek, value);
        }
    }
}

