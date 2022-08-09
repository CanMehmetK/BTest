using System.Text;
using BTest.Infrastructure.Database;
using BTest.Infrastructure.Email;
using BTest.Infrastructure.Identity;
using BTest.Infrastructure.Identity.Entities;
using BTest.Infrastructure.Models;
using BTest.Infrastructure.Store;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BTest.Infrastructure;

public static class ServiceExtensions
{
  public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
  {
    // Add Logging
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<IStoreService, StoreService>();

    services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
    services.AddTransient<IEmailService, EmailService>();

    services.Configure<JWTSettings>(configuration.GetSection(nameof(JWTSettings)));

    services.AddCors(p => p.AddPolicy("corsapp", builder =>
     {
       builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
     }));

    services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
      b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

    services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    services.AddScoped<IUnitOfWork, UnitOfWork>();

    // services.AddTransient<IDbContext, ApplicationDbContext>();

    services.AddIdentity<ApplicationUser, ApplicationRole>()
      .AddEntityFrameworkStores<ApplicationDbContext>()
      .AddDefaultTokenProviders()
      .AddTokenProvider("BTest", typeof(DataProtectorTokenProvider<ApplicationUser>));

    services.AddAuthentication(options =>
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
           ValidIssuer = configuration["JWTSettings:Issuer"],
           ValidAudience = configuration["JWTSettings:Audience"],
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
         };
         o.Events = new JwtBearerEvents()
         {
           OnAuthenticationFailed = c =>
           {
             c.NoResult();
             c.Response.StatusCode = 500;
             c.Response.ContentType = "text/plain";
             return c.Response.WriteAsync(c.Exception.ToString());
           },
           OnChallenge = context =>
           {
             context.HandleResponse();
             context.Response.StatusCode = 401;
             context.Response.ContentType = "application/json";
             var result = JsonConvert.SerializeObject(new BaseResponse<string>("You are not Authorized"));
             return context.Response.WriteAsync(result);
           },
           OnForbidden = context =>
           {
             context.Response.StatusCode = 403;
             context.Response.ContentType = "application/json";
             var result = JsonConvert.SerializeObject(new BaseResponse<string>("You are not authorized to access this resource"));
             return context.Response.WriteAsync(result);
           },
         };
       });

    services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

    services.AddAutoMapper(typeof(MappingProfiles));

    services.AddAuthorization(options =>
    {
      options.AddPolicy("OnlyAdmins", policy => policy.RequireRole("SuperAdmin", "Admin"));
    });
    //services.AddRouting(options => options.LowercaseUrls = true);
  }
}

