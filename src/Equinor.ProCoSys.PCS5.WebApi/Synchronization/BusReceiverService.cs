using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Equinor.ProCoSys.PCS5.Domain;
using Equinor.ProCoSys.PCS5.Domain.AggregateModels.ProjectAggregate;
using Equinor.ProCoSys.PcsServiceBus;
using Equinor.ProCoSys.PcsServiceBus.Receiver.Interfaces;
using Equinor.ProCoSys.PcsServiceBus.Topics;
using Microsoft.Extensions.Options;
using Equinor.ProCoSys.PCS5.WebApi.Authentication;
using Equinor.ProCoSys.Auth.Authentication;
using Equinor.ProCoSys.Common.Misc;
using Equinor.ProCoSys.Common.Telemetry;

namespace Equinor.ProCoSys.PCS5.WebApi.Synchronization
{
    public class BusReceiverService : IBusReceiverService
    {
        private readonly IPlantSetter _plantSetter;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITelemetryClient _telemetryClient;
        private readonly IMainApiAuthenticator _mainApiAuthenticator;
        private readonly ICurrentUserSetter _currentUserSetter;
        private readonly IProjectRepository _projectRepository;
        private readonly Guid _fooApiOid;
        private readonly string FooBusReceiverTelemetryEvent = "FOO Bus Receiver";

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
            _fooApiOid =  options.Value.PCS5ApiObjectId;
        }

        public async Task ProcessMessageAsync(PcsTopic pcsTopic, string messageJson, CancellationToken cancellationToken)
        {
            // >> next 2 lines needed if processing messages lead to requests to foreign api's
            _currentUserSetter.SetCurrentUserOid(_fooApiOid);
            _mainApiAuthenticator.AuthenticationType = AuthenticationType.AsApplication;
            // <<

            switch (pcsTopic)
            {
                case PcsTopic.Project:
                    await ProcessProjectEvent(messageJson);
                    break;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task ProcessProjectEvent(string messageJson)
        {
            var projectEvent = JsonSerializer.Deserialize<ProjectTmpTopic>(messageJson);
            if (projectEvent != null && projectEvent.Behavior == "delete")
            {
                TrackDeleteEvent(PcsTopic.Project, projectEvent.ProCoSysGuid, false);
                return;
            }
            if (projectEvent == null || projectEvent.Plant.IsEmpty() || projectEvent.ProjectName.IsEmpty())
            {
                throw new ArgumentNullException($"Deserialized JSON is not a valid ProjectEvent {messageJson}");
            }

            TrackProjectEvent(projectEvent);

            _plantSetter.SetPlant(projectEvent.Plant);

            var project = await _projectRepository.GetProjectOnlyByNameAsync(projectEvent.ProjectName);
            if (project != null)
            {
                project.Description = projectEvent.Description;
                project.IsClosed = projectEvent.IsClosed;
            }
        }

        private void TrackProjectEvent(ProjectTmpTopic projectEvent) =>
            _telemetryClient.TrackEvent(FooBusReceiverTelemetryEvent,
                new Dictionary<string, string>
                {
                    {"Event", ProjectTopic.TopicName},
                    {nameof(projectEvent.ProCoSysGuid), projectEvent.ProCoSysGuid},
                    {nameof(projectEvent.ProjectName), projectEvent.ProjectName},
                    {nameof(projectEvent.IsClosed), projectEvent.IsClosed.ToString()},
                    {nameof(projectEvent.Plant), projectEvent.Plant[4..]}
                });

        private void TrackDeleteEvent(PcsTopic topic, string guid, bool supported) =>
            _telemetryClient.TrackEvent(FooBusReceiverTelemetryEvent,
                new Dictionary<string, string>
                {
                    {"Event Delete", topic.ToString()},
                    {"ProCoSysGuid", guid},
                    {"Supported", supported.ToString()}
                });
    }
}
