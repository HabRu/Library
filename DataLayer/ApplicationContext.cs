using Library.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Library.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        private readonly ILogger<ApplicationContext> logger;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, ILogger<ApplicationContext> logger)
            : base(options)
        {
            this.logger = logger;
            //Миграция в бд
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

                modelBuilder.Entity<Book>()
                    .Property(b => b.Id)
                    .HasIdentityOptions(startValue: 5);
                modelBuilder.Entity<Evaluation>()
                    .Property(e => e.Id)
                    .HasIdentityOptions(startValue: 5);

                modelBuilder.InitializeData();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            base.OnModelCreating(modelBuilder);

        }
    }
}
