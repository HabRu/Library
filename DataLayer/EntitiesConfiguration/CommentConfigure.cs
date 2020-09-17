using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Configuration
{
    public class CommentConfigure : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Book).WithMany(b => b.Comments).HasForeignKey(c => c.BookId);

            builder.ToTable("Comments");
        }
    }
}
