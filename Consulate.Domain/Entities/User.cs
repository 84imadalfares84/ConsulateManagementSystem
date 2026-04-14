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
        public Employee Employee { get; set; }//navigation property 
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public bool IsEmailVerified { get; set; }//verificati0n email after registration
        public string? EmailVerificationToken { get; set; }//token to verify email after registration
    }
}

