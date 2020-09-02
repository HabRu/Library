using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Configuration
{
    public class TrackingConfigure : IEntityTypeConfiguration<Tracking>
    {
        public void Configure(EntityTypeBuilder<Tracking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.User).WithMany(u => u.TrackingList).HasForeignKey(b => b.UserId);
            builder.HasOne(b => b.Book).WithMany(book => book.TrackingList).HasForeignKey(b => b.BookId);

            builder.ToTable("Trackings");
        }
    }
}
