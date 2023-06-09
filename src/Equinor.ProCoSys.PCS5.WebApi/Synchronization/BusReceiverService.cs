using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PcsServiceBus.Receiver.Interfaces;
using Equinor.ProCoSys.PcsServiceBus.Topics;
using Microsoft.Extensions.Options;
using Equinor.ProCoSys.PCS5.WebApi.Authentication;
using Equinor.ProCoSys.Auth.Authentication;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.Common.Telemetry;

namespace Equinor.ProCoSys.PCS5.WebApi.Synchronization;

public class BusReceiverService : IBusReceiverService
{
    private readonly IPlantSetter _plantSetter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITelemetryClient _telemetryClient;
    private readonly IMainApiAuthenticator _mainApiAuthenticator;
    private readonly ICurrentUserSetter _currentUserSetter;
    private readonly IProjectRepository _projectRepository;
    private readonly Guid _fooApiObjectid;
    private readonly string _fooBusReceiverTelemetryEvent = "FOO Bus Receiver";

    public BusReceiverService(
        IPlantSetter plantSetter,
        IUnitOfWork unitOfWork,
        ITelemetryClient telemetryClient,
        IMainApiAuthenticator mainApiAuthenticator,
        IOptionsSnapshot<PCS5AuthenticatorOptions> options,
        ICurrentUserSetter currentUserSetter,
        IProjectRepository projectRepository)
    {
        _plantSetter = plantSetter;
        _unitOfWork = unitOfWork;
        _telemetryClient = telemetryClient;
        _mainApiAuthenticator = mainApiAuthenticator;
        _currentUserSetter = currentUserSetter;
        _projectRepository = projectRepository;
        _fooApiObjectid =  options.Value.PCS5ApiObjectId;
    }

    public async Task ProcessMessageAsync(string pcsTopic, string messageJson, CancellationToken cancellationToken)
    {
        // >> next 2 lines needed if processing messages lead to requests to foreign api's
        _currentUserSetter.SetCurrentUserOid(_fooApiObjectid);
        _mainApiAuthenticator.AuthenticationType = AuthenticationType.AsApplication;
        // <<

        switch (pcsTopic.ToLower())
        {
            case ProjectTopic.TopicName:
                await ProcessProjectEvent(messageJson);
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ProcessProjectEvent(string messageJson)
    {
        var projectEvent = JsonSerializer.Deserialize<ProjectTopic>(messageJson);
        if (projectEvent == null || projectEvent.Plant.IsEmpty())
        {
            throw new ArgumentNullException($"Deserialized JSON is not a valid ProjectEvent {messageJson}");
        }

        TrackProjectEvent(projectEvent);

        _plantSetter.SetPlant(projectEvent.Plant);

        var project = await _projectRepository.TryGetByGuidAsync(projectEvent.ProCoSysGuid);
        if (project != null)
        {
            // todo The softstring "delete" should be defined in Equinor.ProCoSys.PcsServiceBus
            if (projectEvent.Behavior == "delete")
            {
                project.IsDeletedInSource = true;
                return;
            }

            project.Name = projectEvent.ProjectName;
            project.Description = projectEvent.Description;
            project.IsClosed = projectEvent.IsClosed;
        }
        else
        {
            if (projectEvent.Behavior == "delete")
            {
                return;
            }

            // todo add unit test
            project = new Project(
                projectEvent.Plant,
                projectEvent.ProCoSysGuid,
                projectEvent.ProjectName,
                projectEvent.Description);
            _projectRepository.Add(project);
        }
    }

    private void TrackProjectEvent(ProjectTopic projectEvent) =>
        _telemetryClient.TrackEvent(_fooBusReceiverTelemetryEvent,
            new Dictionary<string, string?>
            {
                {"Event", ProjectTopic.TopicName},
                {nameof(projectEvent.Behavior), projectEvent.Behavior},
                {nameof(projectEvent.ProCoSysGuid), projectEvent.ProCoSysGuid.ToString()},
                {nameof(projectEvent.ProjectName), projectEvent.ProjectName},
                {nameof(projectEvent.IsClosed), projectEvent.IsClosed.ToString()},
                {nameof(projectEvent.Plant), projectEvent.Plant[4..]}
            });
}
