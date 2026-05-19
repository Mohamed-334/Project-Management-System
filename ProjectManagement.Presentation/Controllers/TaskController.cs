using ProjectManagement.Core.Features.Tasks.Commands.RequestModels;
using ProjectManagement.Core.Features.Tasks.Queries.RequestModels;
using ProjectManagement.Domain.Meta;
using ProjectManagement.Presentation.Shared.BaseController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Presentation.Controllers
{
    [ApiController]
    [Authorize]
    public class TaskController : BaseControllerApp
    {
        [HttpGet(Router.TaskRouting.GetById)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetTaskByIdQueryRequestModel()
            {
                Id = id
            });
            return Result(response);
        }

        [HttpGet(Router.TaskRouting.GetList)]
        public async Task<IActionResult> GetList()
        {
            var response = await _mediator.Send(new GetTasksListQueryRequestModel());
            return Result(response);
        }

        [HttpGet(Router.TaskRouting.GetDropDownList)]
        public async Task<IActionResult> GetDropDownList()
        {
            var response = await _mediator.Send(new GetTasksDropDownQueryRequestModel());
            return Result(response);
        }

        [HttpPost(Router.TaskRouting.GetPaginatedList)]
        public async Task<IActionResult> GetPaginatedList([FromBody] GetTasksPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpPost(Router.TaskRouting.GetProjectTasksPaginatedList)]
        public async Task<IActionResult> GetProjectTasksPaginatedList([FromBody] GetProjectTasksPaginatedListQueryRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpPost(Router.TaskRouting.Create)]
        public async Task<IActionResult> Create([FromBody] AddTaskCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpPut(Router.TaskRouting.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskCommandRequestModel request)
        {
            var response = await _mediator.Send(request);
            return Result(response);
        }

        [HttpDelete(Router.TaskRouting.HardDelete)]
        public async Task<IActionResult> HardDelete([FromRoute] int id)
        {
            var response = await _mediator.Send(new DeleteTaskCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }

        [HttpGet(Router.TaskRouting.SoftDeleteAndActivate)]
        public async Task<IActionResult> SoftDeleteAndActivate([FromRoute] int id)
        {
            var response = await _mediator.Send(new SoftDeleteAndActivateTaskCommandRequestModel
            {
                Id = id
            });
            return Result(response);
        }
    }
}
