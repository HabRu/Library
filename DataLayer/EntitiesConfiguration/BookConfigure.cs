using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Models.Configuration
{
    public class BookConfigure : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books").HasKey(b => b.Id);
            builder.Property(b => b.Id).IsRequired();
            builder.Property(b => b.Title).IsRequired().HasMaxLength(50);
            builder.Property(b => b.Authtor).IsRequired().HasMaxLength(50);
        }
    }
}
