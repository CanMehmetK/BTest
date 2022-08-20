using BTest.Infrastructure;
using BTest.Infrastructure.Identity;
using BTest.Infrastructure.Database.Seed;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddInfra(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
  FileProvider = new PhysicalFileProvider(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "wwwroot"))
});
app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<BTest.SPA.Middlewares.ErrorHandlerMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

using (var scope = app.Services.CreateScope())
  await scope.ServiceProvider.Seed(app.Configuration);

app.Run();
