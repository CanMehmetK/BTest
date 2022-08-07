using BTest.Infrastructure.Database.Entities;
using BTest.Infrastructure.Identity.Entities;
using BTest.Infrastructure.Identity.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
namespace BTest.Infrastructure.Database.Seed;
public static class ServiceExtensions
{
  public static async Task Seed(this IServiceProvider services, IConfiguration configuration)
  {
    Log.Logger = new LoggerConfiguration()
      .Enrich.FromLogContext()
      .WriteTo.Console()
      .MinimumLevel.Verbose()
      .CreateLogger();

    try
    {
      var dbContext = services.GetRequiredService<ApplicationDbContext>();
      await dbContext.Database.EnsureCreatedAsync();
      if (dbContext.Database.GetPendingMigrations().Any())
        await dbContext.Database.GetAppliedMigrationsAsync();

      var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
      var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
      if (!roleManager.Roles.Any())
      {
        Log.Information("Seeding Default Roles");
        await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.SuperAdmin.ToString(), CreatedDate = DateTime.Now });
        Log.Information(" Role: SuperAdmin");
        await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Admin.ToString(), CreatedDate = DateTime.Now });
        Log.Information(" Role: Admin");
        await roleManager.CreateAsync(new ApplicationRole() { Name = Roles.Basic.ToString(), CreatedDate = DateTime.Now });
        Log.Information(" Role: Basic");
      }
      if (!userManager.Users.Any())
      {
        Log.Information("Seeding Default User");
        var defaultUser = new ApplicationUser
        {
          UserName = "admin@admin.com",
          Email = "admin@admin.com",
          FirstName = "admin",
          LastName = "super",
          EmailConfirmed = true,
          PhoneNumberConfirmed = true
        };
        Log.Information($"Seeding Default User {defaultUser.Email} with pass Pa$$word!123");
        await userManager.CreateAsync(defaultUser, "Pa$$word!123");
        await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
        await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
        await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
      }

      var categoryRepo = services.GetRequiredService<IGenericRepository<Category>>();
      var productRepo = services.GetRequiredService<IGenericRepository<Product>>();
      if (!categoryRepo.Any())
      {
        Log.Information("Seeding Categories, Products, Stocks");
        Category foods = new Category { Name = "Foods", Slug = "food" };
        foods.Products = new List<Product>(){
          new Product{Name = "Apples", Slug = "apples", Description = "Juicy apples",
                Price = 1.50m,
                Image = "f5dc56be-0110-4bee-9a2c-d85ce2835e1d.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Bananas", Slug = "bananas",
                Description = "Fresh bananas", Price = 3M,
                Image = "905d494f-3a0f-4ac2-90f3-e6e50615d552.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Watermelon", Slug = "watermelon",
                Description = "Juicy watermelon", Price = 0.50M,
                Image = "0511cf13-aeeb-4a41-b7c2-4c5c9876bf1c.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Grapefruit", Slug = "grapefruit",
                Description = "Juicy grapefruit", Price = 2M,
                Image = "7092f635-8bb8-4759-90f3-172987defb77.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Grapes", Slug = "grapes", Description = "Grapes",
                Price = 2M, Image = "a84a4202-9d2c-4b35-a9ef-10a11498cf26.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Kiwi", Slug = "kiwi", Description = "Kiwis",
                Price = 1.2M,
                Image = "f6d16fac-3425-4598-810c-3cefc0a5e6c8.jpg",
                Stock = new Stock(){Quantity = 10m}},
        };

        Category shirts = new Category { Name = "Shirts", Slug = "shirts" };
        shirts.Products = new List<Product>(){
          new Product{Name = "White shirt", Slug = "white-shirt",
                Description = "White shirt", Price = 5.99M,
                Image = "fb526940-f65a-4fa0-b8a0-6a4089ed9f57.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Black shirt", Slug = "black-shirt",
                Description = "Black shirt", Price = 7.99M,
                Image = "cecf805a-0192-44cc-ac48-8d84d8694084.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Yellow shirt", Slug = "yellow-shirt",
                Description = "Yellow shirt", Price = 11.99M,
                Image = "31604614-5a07-4956-9771-96c3ae545119.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Grey shirt", Slug = "grey-shirt",
                Description = "Grey shirt", Price = 12.99M,
                Image = "8e6fb757-6a21-4962-a89f-384cb7275ac4.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Blue shirt", Slug = "blue-shirt",
                Description = "Blue shirt", Price = 12.99M,
                Image = "4929b6e7-5f33-4d37-bb89-86882a32c585.jpg",
                Stock = new Stock(){Quantity = 10m}},
          new Product{Name = "Pink shirt", Slug = "pink-shirt",
                Description = "Pink shirt", Price = 12.99M,
                Image = "a0e240a6-bff6-4935-a77b-169cef279677.jpg",
                Stock = new Stock(){Quantity = 10m}}
        };


        categoryRepo.Insert(foods);
        categoryRepo.Insert(shirts);
      }

      var orderRepo = services.GetRequiredService<IGenericRepository<Order>>();
    }
    catch (Exception ex)
    {

      throw;
    }
  }
}
