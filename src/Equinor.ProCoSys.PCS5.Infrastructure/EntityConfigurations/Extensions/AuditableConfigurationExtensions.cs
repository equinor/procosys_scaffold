using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Equinor.ProCoSys.PCS5.Infrastructure.EntityConfigurations.Extensions;

public static class AuditableConfigurationExtensions
{
    public static void ConfigureCreationAudit<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, ICreationAuditable
    {
        builder
            .Property(x => x.CreatedAtUtc)
            .HasConversion(PCS5Context.DateTimeKindConverter);

        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ConfigureModificationAudit<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IModificationAuditable
    {
        builder
            .Property(x => x.ModifiedAtUtc)
            .HasConversion(PCS5Context.DateTimeKindConverter);

        builder
            .HasOne<Person>()
            .WithMany()
            .HasForeignKey(x => x.ModifiedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}