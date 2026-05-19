using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Commands.RequestModels
{
    public class DeleteCommandRequestModelQuery : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
