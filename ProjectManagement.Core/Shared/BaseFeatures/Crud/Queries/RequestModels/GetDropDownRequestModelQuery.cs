using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using MediatR;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Queries.RequestModels
{
    public class GetDropDownRequestModelQuery : IRequest<Response<List<DropDown>>>
    {
    }
}
