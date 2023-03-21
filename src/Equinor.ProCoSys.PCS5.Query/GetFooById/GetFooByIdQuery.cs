using MediatR;
using ServiceResult;

namespace Equinor.ProCoSys.PCS5.Query.GetFooById
{
    public class GetFooByIdQuery : IRequest<Result<FooDetailsDto>>, IFooQueryRequest
    {
        public GetFooByIdQuery(int fooId) => FooId = fooId;

        public int FooId { get; }
    }
}
