using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem
{
    public static class CurrentSession
    {
        public static Zamestnanec LoggedInUser { get; set; }
    }
}
