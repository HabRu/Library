using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Models.Configuration
{
    public class EvaluationConfigure : IEntityTypeConfiguration<Evaluation>
    {
        public void Configure(EntityTypeBuilder<Evaluation> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Book).WithOne(b => b.Evaluation).HasForeignKey<Evaluation>(e => e.BookId);

            builder.ToTable("Evaluations");
        }
    }
}
