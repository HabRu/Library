using Library.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Library
{
    public static class RoleInitializer
    {
        public static void AddUsersAndRole(this IApplicationBuilder app, IServiceScopeFactory serviceScope)
        {
            using var scope = serviceScope.CreateScope();
            Task t;
            var services = scope.ServiceProvider;
            try
            {
                //Миграция в бд
                var db = services.GetRequiredService<ApplicationContext>();
                db.Database.Migrate();


                var userManager = services.GetRequiredService<UserManager<User>>();
                var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                t = RoleInitializerApp.InitializeAsync(userManager, rolesManager);
                t.Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
