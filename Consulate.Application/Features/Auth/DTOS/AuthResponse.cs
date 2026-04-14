using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Application.Features.Auth.DTOS
{
    public record AuthResponse( //هذا الاستجابة التي سترجع للمستخدم بعد تسجيل الدخول الناجح أو التسجيل الناجح
    string AccessToken,
    string RefreshToken
  
);
}

