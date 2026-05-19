using ProjectManagement.Core.Features.Notifications.Commands.RequestModels;
using ProjectManagement.Core.Features.Notifications.Queries.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Presentation.Controllers
{
    [ApiController]
    public class NotificationController : BaseControllerApp
    {
        [HttpGet(Router.NotificationRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetNotificationByIdQueryRequestModel()
            {
                Id = id
            });
            return Result(response);
        }
        [HttpGet(Router.NotificationRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            var response = await _mediator.Send(new GetNotificationsListQueryRequestModel());
            return Result(response);
        }
        [HttpGet(Router.NotificationRouting.GetDropDownList)]
        public async Task<IActionResult> GetDropDownList()
        {
            var response = await _mediator.Send(new GetNotificationsDropDownQueryRequestModel());
            return Result(response);
        }
        [HttpPost(Router.NotificationRouting.GetPaginatedList)]
        public async Task<IActionResult> GetPaginatedList([FromBody] GetNotificationsPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.NotificationRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddNotificationCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.NotificationRouting.Send)]
        public async Task<IActionResult> Send([FromBody] SendNotificationCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPut(Router.NotificationRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateNotificationCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpDelete(Router.NotificationRouting.HardDelete)]
        public async Task<IActionResult> HardDelete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteNotificationCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }
        [HttpGet(Router.NotificationRouting.SoftDeleteAndActivate)]
        public async Task<IActionResult> SoftDeleteAndActivate([FromRoute] int id)
        {
            var response = await _mediator.Send(new SoftDeleteAndActivateNotificationCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }
    }
}