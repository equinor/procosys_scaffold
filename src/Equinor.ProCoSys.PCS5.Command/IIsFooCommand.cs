using System;
using MediatR;

namespace Equinor.ProCoSys.PCS5.Command;

public interface IIsFooCommand : IBaseRequest
{
    Guid FooGuid { get; }
}
