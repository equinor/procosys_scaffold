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
            .Property(x => x.CreatedAtUtc)
            .HasConversion(PCS5Context.DateTimeKindConverter);

        builder.Property(x => x.SourceType)
            .HasMaxLength(Comment.SourceTypeLengthMax)
            .IsRequired();

        builder.Property(x => x.Text)
            .HasMaxLength(Comment.TextLengthMax)
            .IsRequired();

        builder
            .HasIndex(x => x.SourceGuid)
            .HasDatabaseName("IX_Comments_SourceGuid")
            .IncludeProperties(x => new
            {
                x.Guid,
                x.Text,
                x.CreatedById,
                x.CreatedAtUtc
            });
    }
}
