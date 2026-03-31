using Consulate.Application.Interfaces;
using Consulate.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Consulate.Infrastructure.Services
{
    //token = headers + payload(claims) + signature
    // هذه الخدمة لتوليد رمز JWT (JSON Web Token) الذي يستخدم للمصادقة والتفويض في تطبيقات الويب.
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
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
            //  إضافة Roles
            var roles = user.UserRoles.Select(ur => ur.Role.Name);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // توليد التوكن

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],// الجهة المصدرة من هو السيرفر الذي اصدر هذا التوكن
                audience: _config["Jwt:Audience"],// الجهة المستهدفة من هو المستهلك لهذا التوكن
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(1),// مدة صلاحية التوكن 1 دقيقة
                signingCredentials: creds // بيانات التوقيع التي تم انشاؤها سابقا
            );


            return new JwtSecurityTokenHandler().WriteToken(token);
            //JwtSecurityTokenHandler() كلاس جاهز موجود في مكتبة JWT 
            //يستخدم لكتابة التوكن في شكل نصي يمكن إرساله إلى العميل
            //WriteToken(token) تحويل كائن التوكن (JwtSecurityToken) إلى String يمكن إرساله للمستخدم.
            //بعد هذا السطر تم تحويل التوكن الى نص يمكن إرساله إلى العميل لاستخدامه في الطلبات المستقبلية للمصادقة والتفويض.
        }
        // توليد توكن التحديث الذي يستخدم لتجديد صلاحية التوكن الرئيسي بعد انتهاء صلاحيته
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64));// توليد 64 بايت عشوائية وتحويلها إلى نص Base64 لاستخدامها كتوكين تحديث
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false // مهم جداً
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            return principal; //  لازم يرجع ClaimsPrincipal
        }

    }
    }

