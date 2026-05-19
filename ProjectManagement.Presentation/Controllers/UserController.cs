using ProjectManagement.Core.Features.ApplicationUser.Commands.RequestModels;
using ProjectManagement.Core.Features.ApplicationUser.Queries.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : BaseControllerApp
    {
        [HttpGet(Router.UserRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetUserByIdQueryRequestModel()
            {
                UserId = id
            });
            return Result(response);
        }
        [HttpGet(Router.UserRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            var response = await _mediator.Send(new GetUsersListQueryRequestModel());
            return Result(response);
        }
        [HttpGet(Router.UserRouting.GetUserRoles)]
        public async Task<IActionResult> GetUserRoles([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetUserRolesQueryRequestModel()
            {
                UserId = id
            });
            return Result(response);
        }
        [HttpPost(Router.UserRouting.GetPaginatedList)]
        public async Task<IActionResult> GetPaginatedList([FromBody] GetUsersPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPut(Router.UserRouting.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateUserCommandRequestQuery request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpDelete(Router.UserRouting.HardDelete)]
        public async Task<IActionResult> HardDelete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteUserCommandRequestQuery
            {
                Id = id
            });
            return Result(response);
        }
        [HttpGet(Router.UserRouting.SoftDeleteAndActivate)]
        public async Task<IActionResult> SoftDeleteAndActivate([FromRoute] int id)
        {
            var response = await _mediator.Send(new SoftDeleteAndActivateUserCommandRequestQuery
            {
                Id = id
            });
            return Result(response);
        }
    }
}
