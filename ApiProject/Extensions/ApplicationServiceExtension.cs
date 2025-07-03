using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using ApiProject.Helpers;
using ApiProject.Services;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interceptors;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ApiProject.Helpers.Errors;

namespace ApiProject.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()   //WithOrigins("https://dominio.com")
                    .AllowAnyMethod()          //WithMethods("GET","POST")
                    .AllowAnyHeader());        //WithHeaders("accept","content-type")
            });

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<CreateOrderDetailsService>();
            services.AddScoped<GenerateInvoiceService>();
            //Interceptor
            services.AddScoped<AuditInterceptor>();
            services.AddHttpContextAccessor();

        }

        public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "desconocida";
                    context.HttpContext.Response.StatusCode = 429;
                    context.HttpContext.Response.ContentType = "application/json";
                    var mensaje = $"{{\"message\": \"Demasiadas peticiones desde la IP {ip}. Intenta más tarde.\"}}";
                    await context.HttpContext.Response.WriteAsync(mensaje, token);
                };

                // Aquí no se define GlobalLimiter
                options.AddPolicy("ipLimiter", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(10),
                        QueueLimit = 0,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                });
            });

            return services;
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuration from AppSettings
            services.Configure<JWT>(configuration.GetSection("JWT"));

            //Adding Athentication - JWT
            _ = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                    };
                });
                    // 3. Authorization – Policies
                    services.AddAuthorization(options =>
                    {
                        // Solo Admins
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Administrator"));

                // Solo Mechanics
                options.AddPolicy("MechanicOnly", policy =>
                    policy.RequireRole("Mechanic"));

                // Solo Recepcionista
                options.AddPolicy("RecepcionistOnly", policy =>
                    policy.RequireRole("Recepcionist"));

                // Admins o Mechanics
                options.AddPolicy("AdminOrMechanic", policy =>
                    policy.RequireRole("Administrator", "Mechanic"));

                // Claim de suscripción premium
                options.AddPolicy("PremiumSubscription", policy =>
                    policy.RequireClaim("Subscription", "Premium"));

                // Paciente o Premium (ejemplo compuesto)
                options.AddPolicy("PatientOrPremium", policy =>
                    policy.RequireAssertion(context =>
                        context.User.IsInRole("Patient") ||
                        context.User.HasClaim(c => c.Type == "Subscription" && c.Value == "Premium")));
            });
        }
        public static void AddValidationErrors(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {

                    var errors = actionContext.ModelState.Where(u => u.Value!.Errors.Count > 0)
                                                    .SelectMany(u => u.Value!.Errors)
                                                    .Select(u => u.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidation()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
        }
    }
}