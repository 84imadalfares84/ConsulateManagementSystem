using AutoMapper;
using Consulate.API.Middleware;
using Consulate.Application.Common;
using Consulate.Application.Features.Employees.Commands;
using Consulate.Application.Interfaces;
using Consulate.Application.Mappings;
using Consulate.Infrastructure.Email;
using Consulate.Infrastructure.Persistence;
using Consulate.Infrastructure.Repositories;
using Consulate.Infrastructure.Services;
using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
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
//HangFire 
builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());

builder.Services.AddHangfireServer();
//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "ConsulateApp_";
});
builder.Services.AddScoped<ICacheService, CacheService>();
// AutoMapper
builder.Services.AddAutoMapper(typeof(EmployeeProfile).Assembly);

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
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(
       builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

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

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Routing يجب أن يكون أولاً
app.UseRouting();

// Exception Handling Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// التعامل مع Status Codes مثل 404 و 405 وتحويلها إلى JSON
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    var path = context.HttpContext.Request.Path;

    response.ContentType = "application/json";

    var message = response.StatusCode switch
    {
        404 => "Resource not found",
        405 => "Method Not Allowed",
        _ => "An error occurred"
    };

    await response.WriteAsJsonAsync(new
    {
        StatusCode = response.StatusCode,
        Message = message,
        Path = path
    });
});

// Map Controllers
app.MapControllers();

app.Run();