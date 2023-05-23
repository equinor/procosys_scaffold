using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Equinor.ProCoSys.Auth;
using Equinor.ProCoSys.Common;
using Equinor.ProCoSys.PCS5.Command;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;
using Equinor.ProCoSys.PCS5.Command.FooCommands.CreateLink;
using Equinor.ProCoSys.PCS5.Command.FooCommands.DeleteFoo;
using Equinor.ProCoSys.PCS5.Command.FooCommands.EditFoo;
using Equinor.ProCoSys.PCS5.Command.FooCommands.VoidFoo;
using Equinor.ProCoSys.PCS5.Query.GetFooByGuid;
using Equinor.ProCoSys.PCS5.Query.GetFoosInProject;
using Equinor.ProCoSys.PCS5.WebApi.Middleware;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceResult.ApiExtensions;

namespace Equinor.ProCoSys.PCS5.WebApi.Controllers.Foo;

[ApiController]
[Route("Foos")]
public class FoosController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoosController(IMediator mediator) => _mediator = mediator;

    [AuthorizeAny(Permissions.FOO_READ, Permissions.APPLICATION_TESTER)]
    [HttpGet("{guid}")]
    public async Task<ActionResult<FooDetailsDto>> GetFooByGuid(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromRoute] Guid guid)
    {
        var result = await _mediator.Send(new GetFooByGuidQuery(guid));
        return this.FromResult(result);
    }

    [AuthorizeAny(Permissions.FOO_READ, Permissions.APPLICATION_TESTER)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FooDto>>> GetFoosInProject(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [Required]
        [FromQuery] string projectName,
        [FromQuery] bool includeVoided = false)
    {
        var result = await _mediator.Send(new GetFoosInProjectQuery(projectName, includeVoided));
        return this.FromResult(result);
    }

    [AuthorizeAny(Permissions.FOO_CREATE, Permissions.APPLICATION_TESTER)]
    [HttpPost]
    public async Task<ActionResult<GuidAndRowVersion>> CreateFoo(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromBody] CreateFooDto dto)
    {
        var result = await _mediator.Send(new CreateFooCommand(dto.Title, dto.ProjectName));
        return this.FromResult(result);
    }

    [AuthorizeAny(Permissions.FOO_WRITE, Permissions.APPLICATION_TESTER)]
    [HttpPut("{guid}")]
    public async Task<ActionResult<string>> EditFoo(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromRoute] Guid guid,
        [FromBody] EditFooDto dto)
    {
        var result = await _mediator.Send(
            new EditFooCommand(guid, dto.Title, dto.Text, dto.RowVersion));
        return this.FromResult(result);
    }

    [AuthorizeAny(Permissions.FOO_WRITE, Permissions.APPLICATION_TESTER)]
    [HttpPut("{guid}/Void")]
    public async Task<ActionResult<string>> VoidFoo(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromRoute] Guid guid,
        [FromBody] RowVersionDto dto)
    {
        var result = await _mediator.Send(
            new VoidFooCommand(guid, dto.RowVersion));
        return this.FromResult(result);
    }

    [AuthorizeAny(Permissions.FOO_DELETE, Permissions.APPLICATION_TESTER)]
    [HttpDelete("{guid}")]
    public async Task<ActionResult> DeleteFoo(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromRoute] Guid guid,
        [FromBody] RowVersionDto dto)
    {
        var result = await _mediator.Send(new DeleteFooCommand(guid, dto.RowVersion));
        return this.FromResult(result);
    }

    // todo create integration test
    [AuthorizeAny(Permissions.FOO_ATTACH, Permissions.APPLICATION_TESTER)]
    [HttpPost("{guid}/Link")]
    public async Task<ActionResult<GuidAndRowVersion>> CreateLink(
        [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
        [Required]
        [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
        string plant,
        [FromRoute] Guid guid,
        [FromBody] CreateLinkDto dto)
    {
        var result = await _mediator.Send(new CreateLinkCommand(guid, dto.Title, dto.Url));
        return this.FromResult(result);
    }

    //[HttpGet]
    //public async Task<ActionResult<IEnumerable<LinkDto>>> GetLinksForProCoSysGuid(
    //    [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
    //    [Required]
    //    [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
    //    string plant,
    //    [FromQuery] Guid proCoSysGuid)
    //{
    //    var result = await _mediator.Send(new GetLinksForProCoSysGuidQuery(proCoSysGuid));
    //    return this.FromResult(result);
    //}

    //[HttpPut("{id}")]
    //public async Task<ActionResult<string>> EditLink(
    //    [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
    //    [Required]
    //    [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
    //    string plant,
    //    [FromRoute] Guid proCoSysGuid,
    //    [FromBody] EditLinkDto dto)
    //{
    //    var result = await _mediator.Send(new EditLinkCommand(proCoSysGuid, dto.Title, dto.Url, dto.RowVersion));
    //    return this.FromResult(result);
    //}

    //[HttpDelete("{id}")]
    //public async Task<ActionResult> DeleteLink(
    //    [FromHeader(Name = CurrentPlantMiddleware.PlantHeader)]
    //    [Required]
    //    [StringLength(PlantEntityBase.PlantLengthMax, MinimumLength = PlantEntityBase.PlantLengthMin)]
    //    string plant,
    //    [FromRoute] Guid proCoSysGuid,
    //    [FromBody] RowVersionDto dto)
    //{
    //    var result = await _mediator.Send(new DeleteLinkCommand(proCoSysGuid, dto.RowVersion));
    //    return this.FromResult(result);
    //}
}
