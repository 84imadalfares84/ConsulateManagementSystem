using Consulate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task<Role?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken);
        Task SaveChangesAsync(); // مهم
        Task GenerateRefreshToken();
        Task<User?> GetByVerificationTokenAsync(string token);

    }
}
