using ProjectManagement.Core.Features.Email.Commands.RequestModels;
using ProjectManagement.Core.Shared.Models;
using ProjectManagement.Infrastructure.Shared.Localization;
using ProjectManagement.Service.ServiceInterfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace ProjectManagement.Core.Features.Email.Commands.Handlers
{
    public class EmailCommandHandler : ResponseHandler,
        IRequestHandler<SendEmailCommandRequestModel, Response<string>>
    {
        #region Fields
        private readonly IEmailService _emailsService;
        private readonly IStringLocalizer<AppLocalization> _stringLocalizer;
        #endregion
        #region Constructors
        public EmailCommandHandler(IStringLocalizer<AppLocalization> stringLocalizer,
                                    IEmailService emailsService) : base(stringLocalizer)
        {
            _emailsService = emailsService;
            _stringLocalizer = stringLocalizer;
        }
        #endregion
        #region Handle Functions
        public async Task<Response<string>> Handle(SendEmailCommandRequestModel request, CancellationToken cancellationToken)
        {
            var response = await _emailsService.SendEmailAsync(request.Email, request.Message, request.Subject);
            if (response == _stringLocalizer[AppLocalizationKeys.Success])
                return Success<string>();
            return BadRequest<string>(_stringLocalizer[AppLocalizationKeys.SendEmailFailed]);
        }
        #endregion
    }
}
