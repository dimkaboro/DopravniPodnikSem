using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Models;

namespace DopravniPodnikSem.Models
{
    public class ZamestnanecRole : BaseModel
    {
        private int _zamestnanecId;
        private Role _role;

        public int ZamestnanecId
        {
            get => _zamestnanecId;
            set => SetField(ref _zamestnanecId, value);
        }

        public Role Role
        {
            get => _role;
            set => SetField(ref _role, value);
        }

        // Навигационное свойство
        public virtual Zamestnanec Zamestnanec { get; set; } // Ссылка на Zamestnanec
    }
}
