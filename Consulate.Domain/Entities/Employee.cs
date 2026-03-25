using Consulate.Domain.Comon;

namespace Consulate.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public ICollection<TransactionOwner> TransactionOwners { get; set; }
            = new List<TransactionOwner>();
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
