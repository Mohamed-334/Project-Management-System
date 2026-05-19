using ProjectManagement.Core.Shared.Models;
using MediatR;

namespace ProjectManagement.Core.Features.Authentication.Commands.RequestModels
{
    public class ResetPasswordOtpVerificationCommandRequestModel : OtpVerificationCommandRequestModel, IRequest<Response<string>>
    {
    }
}
