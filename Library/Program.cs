using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Library
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            #region Старая реализация
            //using (var scope = host.Services.CreateScope())
            //{
            //    Task t;
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        //Миграция в бд
            //        var db = services.GetRequiredService<ApplicationContext>();
            //        db.Database.Migrate();


            //        var userManager = services.GetRequiredService<UserManager<User>>();
            //        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            //        t = RoleInitializerApp.InitializeAsync(userManager, rolesManager);
            //        t.Wait();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "An error occurred while seeding the database.");
            //    }

            //} 
            #endregion
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
