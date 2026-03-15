using AutoMapper;
using Consulate.Application.Common;
using Consulate.Application.Features.Employees.Commands;
using Consulate.Application.Interfaces;
using Consulate.Application.Mappings;
using Consulate.Infrastructure.Persistence;
using Consulate.Infrastructure.Repositories;
using Consulate.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(CreateEmployeeCommand).Assembly));

// AutoMapper
object value = builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommandValidator>();

// Pipeline Behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSwaggerGen();

// هذا الكود يخبر التطبيق كيف يتحقق من صحة التوكن المرسل من المستخدم
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,// التحقق من الجهة المصدرة للتوكن
        ValidateAudience = true,// التحقق من الجهة المستهدفة للتوكن
        ValidateLifetime = true,// التحقق من صلاحية التوكن (تاريخ الانتهاء)
        ValidateIssuerSigningKey = true,// التحقق من صحة توقيع التوكن باستخدام المفتاح السري

        ValidIssuer = builder.Configuration["Jwt:Issuer"],// الجهة المصدرة للتوكن (السيرفر الذي اصدر هذا التوكن)
        ValidAudience = builder.Configuration["Jwt:Audience"],// الجهة المستهدفة للتوكن (المستهلك لهذا التوكن)

        IssuerSigningKey = new SymmetricSecurityKey(// المفتاح السري المستخدم لتوقيع التوكن
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();