using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations;

internal class FooConfiguration : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        builder.ConfigurePlant();
        builder.ConfigureCreationAudit();
        builder.ConfigureModificationAudit();
        builder.ConfigureConcurrencyToken();

        builder.ToTable(t => t.IsTemporal());

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Title)
            .HasMaxLength(Foo.TitleLengthMax)
            .IsRequired();

        builder.Property(x => x.Text)
            .HasMaxLength(Foo.TextLengthMax);

        builder
            .HasIndex(x => x.Guid)
            .HasDatabaseName("IX_Foos_Guid")
            .IncludeProperties(x => new
            {
                x.Title,
                x.Text,
                x.ProjectId,
                x.CreatedById,
                x.CreatedAtUtc,
                x.ModifiedById,
                x.ModifiedAtUtc,
                x.IsVoided,
                x.RowVersion
            });

        builder
            .HasIndex(x => x.ProjectId)
            .HasDatabaseName("IX_Foos_ProjectId")
            .IncludeProperties(x => new
            {
                x.Title,
                x.ProjectId,
                x.IsVoided,
                x.RowVersion
            });
    }
}
