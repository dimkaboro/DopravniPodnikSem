using DopravniPodnikSem.Models;
using DopravniPodnikSem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class ZastavkaTrasaViewModel : BaseViewModel
    {
        private readonly ZastavkaTrasa _zastavkaTrasa;

        public ZastavkaTrasaViewModel(ZastavkaTrasa zastavkaTrasa)
        {
            _zastavkaTrasa = zastavkaTrasa ?? throw new ArgumentNullException(nameof(zastavkaTrasa));
        }

        public int ZastavkaTrasaId
        {
            get => _zastavkaTrasa.ZastavkaTrasaId;
            set
            {
                _zastavkaTrasa.ZastavkaTrasaId = value;
                OnPropertyChanged();
            }
        }

        public DateTime CasPrijezdu
        {
            get => _zastavkaTrasa.CasPrijezdu;
            set
            {
                _zastavkaTrasa.CasPrijezdu = value;
                OnPropertyChanged();
            }
        }

        public int JizdaId
        {
            get => _zastavkaTrasa.JizdaId;
            set
            {
                _zastavkaTrasa.JizdaId = value;
                OnPropertyChanged();
            }
        }

        public int ZastavkaId
        {
            get => _zastavkaTrasa.ZastavkaId;
            set
            {
                _zastavkaTrasa.ZastavkaId = value;
                OnPropertyChanged();
            }
        }

        // Навигационные свойства
        public JizdaViewModel JizdaViewModel =>
            _zastavkaTrasa.Jizda != null ? new JizdaViewModel(_zastavkaTrasa.Jizda) : null;

        public ZastavkaViewModel ZastavkaViewModel =>
          _zastavkaTrasa.Zastavka != null ? new ZastavkaViewModel(new ZastavkaRepository(_zastavkaTrasa.Zastavka)) : null; // Исправлено здесь
    }
}
