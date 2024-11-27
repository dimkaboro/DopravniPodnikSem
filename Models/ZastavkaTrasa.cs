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
        private int _jizdaId;
        private int _zastavkaId;

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

        public int JizdaId
        {
            get => _jizdaId;
            set => SetField(ref _jizdaId, value);
        }

        public int ZastavkaId
        {
            get => _zastavkaId;
            set => SetField(ref _zastavkaId, value);
        }

        // Навигационные свойства
        public virtual Jizda Jizda { get; set; }
        public virtual Zastavka Zastavka { get; set; }
    }
}
