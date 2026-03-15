using Consulate.Domain.Comon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public ICollection<UserRole> UserRoles { get; set; }
            = new List<UserRole>();
    }
}

