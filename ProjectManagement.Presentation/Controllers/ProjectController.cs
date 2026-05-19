using ProjectManagement.Core.Features.Projects.Commands.RequestModels;
using ProjectManagement.Core.Features.Projects.Queries.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class ProjectController : BaseControllerApp
    {
        [HttpGet(Router.ProjectRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetProjectByIdQueryRequestModel()
            {
                Id = id
            });
            return Result(response);
        }

        [HttpGet(Router.ProjectRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            var response = await _mediator.Send(new GetProjectsListQueryRequestModel());
            return Result(response);
        }

        [HttpGet(Router.ProjectRouting.GetDropDownList)]
        public async Task<IActionResult> GetDropDownList()
        {
            var response = await _mediator.Send(new GetProjectsDropDownQueryRequestModel());
            return Result(response);
        }

        [HttpPost(Router.ProjectRouting.GetPaginatedList)]
        public async Task<IActionResult> GetPaginatedList([FromBody] GetProjectsPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpPost(Router.ProjectRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddProjectCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpPut(Router.ProjectRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateProjectCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpDelete(Router.ProjectRouting.HardDelete)]
        public async Task<IActionResult> HardDelete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteProjectCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }

        [HttpGet(Router.ProjectRouting.SoftDeleteAndActivate)]
        public async Task<IActionResult> SoftDeleteAndActivate([FromRoute] int id)
        {
            var response = await _mediator.Send(new SoftDeleteAndActivateProjectCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }
    }
}
