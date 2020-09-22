using BusinessLayer.InrefacesRepository;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.ImplementationsRepository
{
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public DbSet<Book> Books { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<Tracking> Trackings { get; set; }

        public DbSet<User> Users { get; set; }

        public DbContext DbContext {
            get
            { 
                return dbContext;
            } 
        }

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            Books = dbContext.Set<Book>();
            Reservations = dbContext.Set<Reservation>();
            Files = dbContext.Set<FileModel>();
            Comments = dbContext.Set<Comment>();
            Evaluations = dbContext.Set<Evaluation>();
            Trackings = dbContext.Set<Tracking>();
            Users = dbContext.Set<User>();
        }
    }
}
