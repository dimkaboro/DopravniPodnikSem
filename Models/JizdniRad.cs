using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class JizdniRad : BaseModel
    {
        private int _jizdniRadId;
        private DateTime _casOdjezdu;
        private DateTime _casPrijezdu;
        private string _den;
        private int _projetaTrasaId;

        public int JizdniRadId
        {
            get => _jizdniRadId;
            set => SetField(ref _jizdniRadId, value);
        }

        public DateTime CasOdjezdu
        {
            get => _casOdjezdu;
            set => SetField(ref _casOdjezdu, value);
        }

        public DateTime CasPrijezdu
        {
            get => _casPrijezdu;
            set => SetField(ref _casPrijezdu, value);
        }

        public string Den
        {
            get => _den;
            set => SetField(ref _den, value);
        }

        public int ProjetaTrasaId
        {
            get => _projetaTrasaId;
            set => SetField(ref _projetaTrasaId, value);
        }

        // Навигационное свойство для связи с ProjetaTrasa
        public virtual ProjetaTrasa ProjetaTrasa { get; set; }
    }
}

