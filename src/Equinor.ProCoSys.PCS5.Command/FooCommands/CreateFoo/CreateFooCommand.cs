using Equinor.ProCoSys.Common;
using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Command.FooCommands.CreateFoo;

public class CreateFooCommand : IRequest<Result<IdAndRowVersion>>, IProjectRequest
{
    public CreateFooCommand(string title, string projectName)
    {
        Title = title;
        ProjectName = projectName;
    }

    public string Title { get; }
    public string ProjectName { get; }
}
