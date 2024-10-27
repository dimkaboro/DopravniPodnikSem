using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DopravniPodnikSem.Models.Enum
{
    public enum Role
    {
        UNLOGIN = 0,
        ZAKAZNIK = 1,
        ZAMESTNANEC = 2,
        ADMIN = 3
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
                throw new ArgumentException("Wrong role id");
            }
        }
    }
}
