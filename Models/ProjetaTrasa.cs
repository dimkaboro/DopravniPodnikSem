﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models
{
    public class ProjetaTrasa : BaseModel
    {
        private int _projetaTrasaId;
        private DateTime _casStart;
        private DateTime _casEnd;

        public int ProjetaTrasaId
        {
            get => _projetaTrasaId;
            set => SetField(ref _projetaTrasaId, value);
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

        public virtual ICollection<JizdniRad> JizdniRady { get; set; }
    }
}

