using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Domain.Shared.BaseEntity.Implementations;
using MediatR;

namespace ProjectManagement.Core.Features.Notifications.Queries.RequestModels
{
    public class GetNotificationsDropDownQueryRequestModel : IRequest<Response<List<DropDown>>>
    {
    }
}
