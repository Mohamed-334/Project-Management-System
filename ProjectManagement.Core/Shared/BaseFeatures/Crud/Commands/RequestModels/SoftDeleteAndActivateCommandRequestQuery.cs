using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Shared.BaseFeatures.Crud.Commands.RequestModels
{
    public class SoftDeleteAndActivateCommandRequestQuery : IRequest<Response<string>>
    {
        public int Id { get; set; }
    }
}
