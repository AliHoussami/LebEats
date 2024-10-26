using Microsoft.EntityFrameworkCore;
using TEST2.Models;

namespace TEST2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();

            // Enable session services
            builder.Services.AddSession();

            // Database connection
            var connectionString = "Server=localhost;Database=projet;User=root;Password=Alinx123@;Port=3306;SslMode=none;";
            builder.Services.AddDbContext<YourDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();


            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable sessions
            app.UseSession();

            app.UseAuthorization();

            // Set default route to the login page
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
