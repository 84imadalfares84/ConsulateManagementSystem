using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public Guid UserId { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }//خاص بال Rotation لالغاء التوكن القديم بعد استخدامه في الحصول على توكن جديد

        public User User { get; set; } = null!;
        public bool IsRevoked { get; set; }//خاص بال logoff
        public DateTime? RevokedAt { get; set; }
    }
}
