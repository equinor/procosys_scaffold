using Equinor.ProCoSys.PCS5.Domain.AggregateModels.AttachmentAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations;

internal class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ConfigureCreationAudit();
        builder.ConfigureConcurrencyToken();

        builder
            .Property(x => x.CreatedAtUtc)
            .HasConversion(PCS5Context.DateTimeKindConverter);

        builder.Property(x => x.SourceType)
            .HasMaxLength(Attachment.SourceTypeLengthMax)
            .IsRequired();

        builder.Property(x => x.FileName)
            .HasMaxLength(Attachment.FileNameLengthMax)
            .IsRequired();

        builder.Property(x => x.BlobPath)
            .HasMaxLength(Attachment.BlobPathLengthMax)
            .IsRequired();

        builder
            .HasIndex(x => x.SourceGuid)
            .HasDatabaseName("IX_Attachments_SourceGuid")
            .IncludeProperties(x => new
            {
                x.Guid,
                x.FileName,
                x.CreatedById,
                x.CreatedAtUtc,
                x.ModifiedById,
                x.ModifiedAtUtc,
                x.RowVersion
            });
    }
}
