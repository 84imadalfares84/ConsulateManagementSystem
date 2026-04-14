using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string token);
        Task SendLoginNotificationEmail(string email);
    }
}
