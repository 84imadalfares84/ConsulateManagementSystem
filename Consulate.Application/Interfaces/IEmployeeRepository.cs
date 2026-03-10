using Consulate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee employee);
        Task<Employee> GetByIdAsync(Guid id);
        Task <IEnumerable<Employee>> GetAllAsync();
        Task UpdateAsync(Employee employee);
        

    }
}
