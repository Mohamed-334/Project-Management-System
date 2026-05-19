using ProjectManagement.Core.Features.Roles.Commands.RequestModels;
using ProjectManagement.Core.Features.Roles.Queries.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class RoleController : BaseControllerApp
    {
        [HttpGet(Router.RoleRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetRoleByIdQueryRequestModel()
            {
                RoleId = id
            });
            return Result(response);
        }
        [HttpGet(Router.RoleRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            var response = await _mediator.Send(new GetRolesListQueryRequestModel());
            return Result(response);
        }
        [HttpPost(Router.RoleRouting.GetPaginatedList)]
        public async Task<IActionResult> GetPaginatedList([FromBody] GetRolesPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPost(Router.RoleRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddRoleCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpPut(Router.RoleRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }
        [HttpDelete(Router.RoleRouting.Delete)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteRoleCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }
        [HttpGet(Router.RoleRouting.SoftDeleteAndActivate)]
        public async Task<IActionResult> SoftDeleteAndActivate([FromRoute] int id)
        {
            var response = await _mediator.Send(new SoftDeleteAndActivateRoleCommandRequestQuery
            {
                Id = id
            });
            return Result(response);
        }
    }
}
