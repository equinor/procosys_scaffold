using System;
using Equinor.ProCoSys.Common.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Test.Common;

namespace Equinor.ProCoSys.PCS5.Command.Tests
{
    [TestClass]
    public abstract class CommandHandlerTestsBase
    {
        protected Mock<ICurrentUserProvider> CurrentUserProviderMock;
        protected const string TestPlant = "TestPlant";
        protected Mock<IUnitOfWork> UnitOfWorkMock;
        protected Mock<IPlantProvider> PlantProviderMock;
        protected ManualTimeProvider _timeProvider;
        protected DateTime _utcNow;

        [TestInitialize]
        public void BaseSetup()
        {
            UnitOfWorkMock = new Mock<IUnitOfWork>();
            PlantProviderMock = new Mock<IPlantProvider>();
            PlantProviderMock
                .Setup(x => x.Plant)
                .Returns(TestPlant);
            _utcNow = new DateTime(2020, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            _timeProvider = new ManualTimeProvider(_utcNow);
            TimeService.SetProvider(_timeProvider);
        }
    }
}
