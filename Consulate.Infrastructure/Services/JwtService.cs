using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Consulate.Infrastructure.Services
{
    //token = headers + payload(claims) + signature
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(User user)
        {
            // انشاء مفتاح سري باستخدام المفتاح الموجود في الإعدادات
            var key = new SymmetricSecurityKey( 
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            // انشاء بيانات التوقيع باستخدام المفتاح السري وخوارزمية HMAC SHA256
            var creds = new SigningCredentials( 
                key,
                SecurityAlgorithms.HmacSha256);
            // انشاء الادعاءات (claims) التي تحتوي على معلومات المستخدم مثل معرفه وبريده الإلكتروني
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
            // انشاء رمز JWT باستخدام البيانات السابقة وتحديد الجهة المصدرة والجهة المستهدفة ومدة الصلاحية

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],// الجهة المصدرة من هو السيرفر الذي اصدر هذا التوكن
                audience: _config["Jwt:Audience"],// الجهة المستهدفة من هو المستهلك لهذا التوكن
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),// مدة صلاحية التوكن 30 دقيقة
                signingCredentials: creds // بيانات التوقيع التي تم انشاؤها سابقا
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //JwtSecurityTokenHandler() كلاس جاهز موجود في مكتبة JWT 
            //يستخدم لكتابة التوكن في شكل نصي يمكن إرساله إلى العميل
            //WriteToken(token) تحويل كائن التوكن (JwtSecurityToken) إلى String يمكن إرساله للمستخدم.
            //بعد هذا السطر تم تحويل التوكن الى نص يمكن إرساله إلى العميل لاستخدامه في الطلبات المستقبلية للمصادقة والتفويض.
        }
    }
}
