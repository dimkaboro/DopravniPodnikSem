using DopravniPodnikSem.Models.Enum;
using DopravniPodnikSem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.ViewModels
{
    public class ZamestnanecRoleViewModel : BaseViewModel
    {
        private ZamestnanecRole _zamestnanecRole;

        public ZamestnanecRoleViewModel(ZamestnanecRole zamestnanecRole)
        {
            _zamestnanecRole = zamestnanecRole;
        }

        public int ZamestnanecId
        {
            get => _zamestnanecRole.ZamestnanecId;
            set
            {
                _zamestnanecRole.ZamestnanecId = value;
                OnPropertyChanged();
            }
        }

        public Role Role
        {
            get => _zamestnanecRole.Role;
            set
            {
                _zamestnanecRole.Role = value;
                OnPropertyChanged();
            }
        }

        public Zamestnanec Zamestnanec
        {
            get => _zamestnanecRole.Zamestnanec;
            set
            {
                _zamestnanecRole.Zamestnanec = value;
                OnPropertyChanged();
            }
        }
    }
}
