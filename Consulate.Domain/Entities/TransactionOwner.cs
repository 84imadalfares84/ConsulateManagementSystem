using Consulate.Domain.Comon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Domain.Entities
{
    public class TransactionOwner : BaseEntity //مالك المعاملة
    {
        public string FullName { get; set; } = string.Empty;

        public string PassportNumber { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public Guid EmployeeId { get; set; } // FK to Employee

        public Employee? Employee { get; set; } // Navigation property to Employee
    }
}
