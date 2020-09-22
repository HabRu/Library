using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.InrefacesRepository
{
    public interface IRepository
    {
        public DbContext DbContext { get; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<FileModel> Files { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Evaluation> Evaluations { get; set; }

        public DbSet<Tracking> Trackings { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
