using Equinor.ProCoSys.PCS5.Domain.AggregateModels.CommentAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations;
internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ConfigureCreationAudit();
        builder.ConfigureConcurrencyToken();

        builder
            .Property(comment => comment.CreatedAtUtc)
            .HasConversion(PCS5Context.DateTimeKindConverter);

        builder.Property(comment => comment.Text)
            .HasMaxLength(Comment.TextLengthMax)
            .IsRequired();

        builder
            .HasIndex(comment => comment.SourceGuid)
            .HasDatabaseName("IX_Comments_SourceGuid")
            .IncludeProperties(comment => new
            {
                comment.Guid,
                comment.Text,
                comment.CreatedById,
                comment.CreatedAtUtc,
                comment.RowVersion
            });
    }
}
