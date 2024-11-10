using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
        public class Jizda : BaseModel
        {
            private int _jizdaId;
            private DateTime _casStart;
            private DateTime _casEnd;
            private string _stav;
            private int _linkaLinkaId;

            public int JizdaId
            {
                get => _jizdaId;
                set => SetField(ref _jizdaId, value);
            }

            public DateTime CasStart
            {
                get => _casStart;
                set => SetField(ref _casStart, value);
            }

            public DateTime CasEnd
            {
                get => _casEnd;
                set => SetField(ref _casEnd, value);
            }

            public string Stav
            {
                get => _stav;
                set => SetField(ref _stav, value);
            }

            public int LinkaLinkaId
            {
                get => _linkaLinkaId;
                set => SetField(ref _linkaLinkaId, value);
            }

            // Навигационное свойство
            public virtual Linka Linka { get; set; } // Связь с Linka
        }
    }

