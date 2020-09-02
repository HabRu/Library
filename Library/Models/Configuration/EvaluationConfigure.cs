using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Configuration
{
    public class EvaluationConfigure : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Book).WithOne(b => b.Evaluation).HasForeignKey<Evaluation>(b=>b.BookId);

            builder.ToTable("Evaluations");
        }
    }
}
