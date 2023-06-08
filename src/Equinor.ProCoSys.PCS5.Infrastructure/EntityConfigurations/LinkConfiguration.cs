using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations;

internal class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ConfigureCreationAudit();
        builder.ConfigureModificationAudit();
        builder.ConfigureConcurrencyToken();

        builder.ToTable(t => t.IsTemporal());

        builder.Property(x => x.Title)
            .HasMaxLength(Link.TitleLengthMax)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasMaxLength(Link.UrlLengthMax)
            .IsRequired();

        builder
            .HasIndex(link=> link.SourceGuid)
            .HasDatabaseName("IX_Links_SourceGuid")
            .IncludeProperties(link => new
            {
                link.Guid,
                link.Url,
                link.Title,
                link.RowVersion
            });
    }
}
