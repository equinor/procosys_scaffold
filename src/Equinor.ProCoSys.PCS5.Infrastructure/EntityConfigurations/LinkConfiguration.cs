using Equinor.ProCoSys.PCS5.Domain.AggregateModels.LinkAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations;

internal class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ConfigurePlant();
        builder.ConfigureCreationAudit();
        builder.ConfigureModificationAudit();
        builder.ConfigureConcurrencyToken();

        builder.Property(x => x.Title)
            .HasMaxLength(Link.TitleLengthMax)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasMaxLength(Link.UrlLengthMax)
            .IsRequired();

        builder
            .HasIndex(p => p.Plant)
            .HasDatabaseName("IX_Links_Plant_ASC")
            .IncludeProperties(p => new { p.Title, p.Url, p.CreatedAtUtc, p.ModifiedAtUtc });

        builder
            .HasIndex(p => p.Title)
            .HasDatabaseName("IX_Links_Title_ASC")
            .IncludeProperties(p => new { p.Plant });
    }
}
