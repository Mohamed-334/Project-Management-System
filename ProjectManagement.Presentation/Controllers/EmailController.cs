using ProjectManagement.Core.Features.Email.Commands.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class EmailController : BaseControllerApp
    {
        [HttpPost(Router.EmailRouting.SendEmail)]
        public async Task<IActionResult> SendEmail([FromQuery] SendEmailCommandRequestModel command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
