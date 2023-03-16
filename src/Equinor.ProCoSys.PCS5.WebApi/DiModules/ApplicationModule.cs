using Equinor.ProCoSys.PCS5.Command.EventHandlers;
using Equinor.ProCoSys.PCS5.Command.Validators.FooValidators;
using Equinor.ProCoSys.PCS5.Command.Validators.RowVersionValidators;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.FooAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.PersonAggregate;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PCS5.ForeignApi.MainApi.Project;
using Equinor.ProCoSys.PCS5.Infrastructure;
using Equinor.ProCoSys.PCS5.Infrastructure.Repositories;
using Equinor.ProCoSys.PCS5.WebApi.Authentication;
using Equinor.ProCoSys.PCS5.WebApi.Authorizations;
using Equinor.ProCoSys.PCS5.WebApi.Misc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Equinor.ProCoSys.Auth.Authentication;
using Equinor.ProCoSys.Auth.Client;
using Equinor.ProCoSys.Common.Caches;
using Equinor.ProCoSys.Common.Email;
using Equinor.ProCoSys.Common.Telemetry;
using Equinor.ProCoSys.Common;

namespace Equinor.ProCoSys.PCS5.WebApi.DIModules
{
    public static class ApplicationModule
    {
        public static void AddApplicationModules(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApplicationOptions>(configuration.GetSection("Application"));
            services.Configure<MainApiOptions>(configuration.GetSection("MainApi"));
            services.Configure<CacheOptions>(configuration.GetSection("CacheOptions"));
            services.Configure<PCS5AuthenticatorOptions>(configuration.GetSection("Authenticator"));

            services.AddDbContext<PCS5Context>(options =>
            {
                var connectionString = configuration.GetConnectionString(PCS5Context.PCS5ContextConnectionStringName);
                options.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            // Hosted services

            // Transient - Created each time it is requested from the service container

            // Scoped - Created once per client request (connection)
            services.AddScoped<ITelemetryClient, ApplicationInsightsTelemetryClient>();
            services.AddScoped<IAccessValidator, AccessValidator>();
            services.AddScoped<IProjectAccessChecker, ProjectAccessChecker>();
            services.AddScoped<IProjectChecker, ProjectChecker>();
            services.AddScoped<IFooHelper, FooHelper>();
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            services.AddScoped<IUnitOfWork>(x => x.GetRequiredService<PCS5Context>());
            services.AddScoped<IReadOnlyContext, PCS5Context>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IFooRepository, FooRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();

            services.AddScoped<IAuthenticatorOptions, AuthenticatorOptions>();
            services.AddScoped<IProjectApiService, MainApiProjectService>();

            services.AddScoped<IFooValidator, FooValidator>();
            services.AddScoped<IRowVersionValidator, RowVersionValidator>();

            // Singleton - Created the first time they are requested
            services.AddSingleton<IEmailService, EmailService>();
        }
    }
}
