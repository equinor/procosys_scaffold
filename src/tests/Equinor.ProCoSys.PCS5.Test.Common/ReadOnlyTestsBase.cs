using System;
using System.Linq;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.Common.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Infrastructure;

namespace Equinor.ProCoSys.PCS5.Test.Common
{
    public abstract class ReadOnlyTestsBase
    {
        protected readonly string TestPlant = "PCS$PlantA";
        protected Project _project;
        protected Person _currentPerson;
        protected readonly Guid CurrentUserOid = new ("12345678-1234-1234-1234-123456789123");
        protected DbContextOptions<PCS5Context> _dbContextOptions;
        protected Mock<IPlantProvider> _plantProviderMock;
        protected IPlantProvider _plantProvider;
        protected ICurrentUserProvider _currentUserProvider;
        protected IEventDispatcher _eventDispatcher;
        protected ManualTimeProvider _timeProvider;

        [TestInitialize]
        public void SetupBase()
        {
            _project = new(TestPlant, "Proj", "Desc");
            _plantProviderMock = new Mock<IPlantProvider>();
            _plantProviderMock.SetupGet(x => x.Plant).Returns(TestPlant);
            _plantProvider = _plantProviderMock.Object;

            var currentUserProviderMock = new Mock<ICurrentUserProvider>();
            currentUserProviderMock.Setup(x => x.GetCurrentUserOid()).Returns(CurrentUserOid);
            _currentUserProvider = currentUserProviderMock.Object;

            var eventDispatcher = new Mock<IEventDispatcher>();
            _eventDispatcher = eventDispatcher.Object;

            _timeProvider = new ManualTimeProvider(new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc));
            TimeService.SetProvider(_timeProvider);

            _dbContextOptions = new DbContextOptionsBuilder<PCS5Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // ensure current user exists in db. Will be used when setting createdby / modifiedby
            using (var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider))
            {
                if (context.Persons.SingleOrDefault(p => p.Oid == CurrentUserOid) == null)
                {
                    _currentPerson = new Person(CurrentUserOid, "Ole", "Lukkøye", "ol", "ol@pcs.pcs");
                    AddPerson(context, _currentPerson);
                    AddProject(context, _project);
                }
            }

            SetupNewDatabase(_dbContextOptions);
        }

        protected abstract void SetupNewDatabase(DbContextOptions<PCS5Context> dbContextOptions);

        protected Project GetProjectById(int projectId)
        {
            using var context = new PCS5Context(_dbContextOptions, _plantProvider, _eventDispatcher, _currentUserProvider);
            return context.Projects.Single(x => x.Id == projectId);
        }

        protected Person AddPerson(PCS5Context context, Person person)
        {
            context.Persons.Add(person);
            context.SaveChangesAsync().Wait();
            return person;
        }

        protected Project AddProject(PCS5Context context, Project project)
        {
            context.Projects.Add(project);
            context.SaveChangesAsync().Wait();
            return project;
        }
    }
}
