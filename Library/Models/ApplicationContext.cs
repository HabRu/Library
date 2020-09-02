using Library.Models.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<FileModel> Files { get; set; }
        
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<Tracking> Trackings { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Миграция в бд
            //Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfigure())
                        .ApplyConfiguration(new CommentConfigure())
                        .ApplyConfiguration(new EvaluationConfigure())
                        .ApplyConfiguration(new ReservationConfigure())
                        .ApplyConfiguration(new TrackingConfigure());

            base.OnModelCreating(modelBuilder);

        }
    }
}
