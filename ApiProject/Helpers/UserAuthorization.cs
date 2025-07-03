using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Helpers
{
    public class UserAuthorization
    {
        public enum Roles
        {
            Administrator,
            Receptionist,
            Mechanic
        }

        public const Roles rol_predeterminado = Roles.Administrator;
    }
}