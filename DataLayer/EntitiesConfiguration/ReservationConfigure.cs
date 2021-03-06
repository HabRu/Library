﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Models.Configuration
{
    public class ReservationConfigure : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasOne(b => b.Book).WithMany(b => b.Reservations).HasForeignKey(r => r.BookIdentificator);
            builder.HasOne(b => b.User).WithMany(u => u.ReservUser).HasForeignKey(b => b.UserId);

            builder.ToTable("Reservations").HasKey(b => b.Id);
        }
    }
}
