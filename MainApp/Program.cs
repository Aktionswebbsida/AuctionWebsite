
using Data.DbContext;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ApplicationDbContext = Data.DbContext.ApplicationDbContext;

namespace MainApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContexts>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                      .AddRoles<IdentityRole<int>>()
                      .AddEntityFrameworkStores<ApplicationDbContexts>();


        builder.Services.AddTransient<DataInitializer>();



        builder.Services.AddHttpClient("APIClient", client =>
        {
            client.BaseAddress = new Uri("https://localhost:7284");
        });


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var inilizer = scope.ServiceProvider.GetRequiredService<DataInitializer>();
            await inilizer.SeedData();
        }
        app.UseStaticFiles();
        app.UseRequestLocalization();
        app.UseHttpsRedirection();

        app.UseRouting();

        // Ensure authentication middleware runs before authorization so Identity can validate users
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }
}