using Microsoft.EntityFrameworkCore;
using ProductApp.Data;
using System.Globalization;

namespace ProductApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Utworzenie builder'a aplikacji, który pozwoli rejestrowaæ us³ugi i konfigurowaæ potok HTTP
            var builder = WebApplication.CreateBuilder(args);

            // Rejestracja MVC
            builder.Services.AddControllersWithViews();
            // Rejestracja HttpClienta do komunikacji z zewnêtrznymi API
            builder.Services.AddHttpClient();

            // Rejestracja DbContext z EF Core i SQLite
            builder.Services.AddDbContext<ProductContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("ProductContext")));

            var app = builder.Build();

            // Konfiguracja potoku HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Definiowanie domyœlnej trasê dla kontrolerów MVC
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Ustawienie kultury (wa¿ne dla poprawnej walidacji decimal z przecinkiem)
            var cultureInfo = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.Run();
            
        }
    }
}
