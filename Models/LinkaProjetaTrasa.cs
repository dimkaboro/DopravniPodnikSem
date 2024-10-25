using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class LinkaProjetaTrasa : BaseModel
    {
        private int _linkaId;
        private int _projetaTrasaId;

        public int LinkaId
        {
            get => _linkaId;
            set => SetField(ref _linkaId, value);
        }

        public int ProjetaTrasaId
        {
            get => _projetaTrasaId;
            set => SetField(ref _projetaTrasaId, value);
        }

        // Навигационные свойства для связи с Linka и ProjetaTrasa
        public virtual Linka Linka { get; set; }
        public virtual ProjetaTrasa ProjetaTrasa { get; set; }
    }
}
