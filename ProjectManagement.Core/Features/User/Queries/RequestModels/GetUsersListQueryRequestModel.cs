using ProjectManagement.Core.Features.ApplicationUser.DTO;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels
{
    public class GetUsersListQueryRequestModel : IRequest<Response<List<UserFullDataDto>>>
    {
    }
}
