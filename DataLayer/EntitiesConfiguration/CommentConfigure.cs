﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Models.Configuration
{
    public class CommentConfigure : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(b => b.Book).WithMany(b => b.Comments).HasForeignKey(c => c.BookId);

            builder.ToTable("Comments").HasKey(b => b.Id); ;
        }
    }
}
