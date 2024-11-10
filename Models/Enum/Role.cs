using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models.Enum
{
    public enum Role
    {
        Guest = 3,
        Zamestnanec = 2,
        Administrator = 1
    }

    public static class RoleService
    {
        public static Role GetRoleById(int roleId)
        {
            if (Role.IsDefined(typeof(Role), roleId))
            {
                return (Role)roleId;
            }
            else
            {
                throw new ArgumentException("Neplatný identifikátor role");
            }
        }
    }
}

