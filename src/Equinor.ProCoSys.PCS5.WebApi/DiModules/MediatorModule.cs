using System.Reflection;
using Equinor.ProCoSys.PCS5.Command;
using Equinor.ProCoSys.PCS5.Query;
using Equinor.ProCoSys.PCS5.WebApi.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Equinor.ProCoSys.PCS5.WebApi.DIModules;

public static class MediatorModule
{
    public static void AddMediatrModules(this IServiceCollection services)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssemblies(
            typeof(MediatorModule).GetTypeInfo().Assembly,
            typeof(ICommandMarker).GetTypeInfo().Assembly,
            typeof(IQueryMarker).GetTypeInfo().Assembly
        ));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckValidProjectBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CheckAccessBehavior<,>));
    }
}