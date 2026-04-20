using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.DTOS
{
    public class UserCacheDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsVerified { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
