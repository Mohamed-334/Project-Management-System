using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Queries.RequestModels
{
    public class GetByIdRequestModelQuery<TDto> : BaseModel, IRequest<Response<TDto>>
    {
    }
}
