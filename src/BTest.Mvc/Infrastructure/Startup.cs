using BTest.Infrastructure.Interfaces;
using BTest.Infrastructure.Mailing;
using BTest.Infrastructure.Models;
using BTest.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BTest.Infrastructure
{
  public static class Startup
  {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
      // Session
      services.AddDistributedMemoryCache();
      services.AddSession(options =>
      {
        options.IdleTimeout = TimeSpan.FromMinutes(20);
        options.Cookie.IsEssential = true;
      });

      // Email Configurations
      services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
      services.AddTransient<IMailService, SmtpMailService>();

      // EF Db Context
      var connectionString = configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

      // MsIdentity With EF
      services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
      {
        options.SignIn.RequireConfirmedAccount = true;
        options.SignIn.RequireConfirmedEmail = true;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters = "abcçdefghiıjklmnoöpqrsştuüvwxyzABCÇDEFGHIİJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._@+'#!/^%{}*";

      })
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders(); ;

    }
  }
}
