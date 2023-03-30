using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.Common.Misc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Equinor.ProCoSys.PCS5.Infrastructure.Tests;

public class ContextHelper
{
    public ContextHelper()
    {
        DbOptions = new DbContextOptions<PCS5Context>();
        EventDispatcherMock = new Mock<IEventDispatcher>();
        PlantProviderMock = new Mock<IPlantProvider>();
        CurrentUserProviderMock = new Mock<ICurrentUserProvider>();
        ContextMock = new Mock<PCS5Context>(
            DbOptions,
            PlantProviderMock.Object,
            EventDispatcherMock.Object,
            CurrentUserProviderMock.Object);
    }

    public DbContextOptions<PCS5Context> DbOptions { get; }
    public Mock<IEventDispatcher> EventDispatcherMock { get; }
    public Mock<IPlantProvider> PlantProviderMock { get; }
    public Mock<PCS5Context> ContextMock { get; }
    public Mock<ICurrentUserProvider> CurrentUserProviderMock { get; }
}