using System;
using System.Threading;
using System.Threading.Tasks;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Equinor.ProCoSys.PCS5.WebApi.Seeding
{
    public class Seeder : IHostedService
    {
        private static readonly Person s_seederUser = new (new Guid("12345678-1234-1234-1234-123456789123"), "Angus", "MacGyver", "am", "am@pcs.pcs");
        private readonly IServiceScopeFactory _serviceProvider;

        public Seeder(IServiceScopeFactory serviceProvider) => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var plantProvider = new SeedingPlantProvider("PCS$SEED");

                using (var dbContext = new PCS5Context(
                    scope.ServiceProvider.GetRequiredService<DbContextOptions<PCS5Context>>(),
                    plantProvider,
                    scope.ServiceProvider.GetRequiredService<IEventDispatcher>(),
                    new SeederUserProvider()))
                {
                    // If the seeder user exists in the database, it's already been seeded. Don't seed again.
                    if (await dbContext.Persons.AnyAsync(p => p.Oid == s_seederUser.Oid))
                    {
                        return;
                    }

                    /* 
                     * Add the initial seeder user. Don't do this through the UnitOfWork as this expects/requires the current user to exist in the database.
                     * This is the first user that is added to the database and will not get "Created" and "CreatedBy" data.
                     */
                    dbContext.Persons.Add(s_seederUser);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    var personRepository = new PersonRepository(dbContext);

                    personRepository.AddUsers(250);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private class SeederUserProvider : ICurrentUserProvider
        {
            public Guid GetCurrentUserOid() => s_seederUser.Oid;
            public bool HasCurrentUser => throw new NotImplementedException();
        }
    }
}
