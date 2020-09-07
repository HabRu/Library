using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Configuration
{
    public class ReservationConfigure : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Book).WithOne(b => b.Reservation).HasForeignKey<Reservation>(r => r.BookIdentificator);
            builder.HasOne(b => b.User).WithMany(u => u.ReservUser).HasForeignKey(b => b.UserId);

            builder.ToTable("Reservations");
        }
    }
}
