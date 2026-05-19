using ProjectManagement.Core.Features.ApplicationUser.DTO;
using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels
{
    public class GetUserByIdQueryRequestModel : IRequest<Response<UserFullDataDto>>
    {
        public int UserId { get; set; }

    }
}
