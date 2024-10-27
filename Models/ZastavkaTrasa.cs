using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class ZastavkaTrasa : BaseModel
    {
        private int _zastavkaTrasaId;
        private DateTime _casPrijezdu;
        private DateTime _casOdjezdu;
        private int _linkaId;

        public int ZastavkaTrasaId
        {
            get => _zastavkaTrasaId;
            set => SetField(ref _zastavkaTrasaId, value);
        }

        public DateTime CasPrijezdu
        {
            get => _casPrijezdu;
            set => SetField(ref _casPrijezdu, value);
        }

        public DateTime CasOdjezdu
        {
            get => _casOdjezdu;
            set => SetField(ref _casOdjezdu, value);
        }

        public int LinkaId
        {
            get => _linkaId;
            set => SetField(ref _linkaId, value);
        }

        // Навигационное свойство для связи с Linka
        public virtual Linka Linka { get; set; }
    }
}
