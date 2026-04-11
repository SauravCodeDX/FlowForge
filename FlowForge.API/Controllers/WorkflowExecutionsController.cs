using FlowForge.Application.WorkflowExecutions;
using FlowForge.Application.WorkflowExecutions.Queries.GetWorkflowExecutionById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowExecutionsController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkflowExecutionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WorkflowExecutionDto>> GetWorkflowExecutionById(Guid id, CancellationToken cancellationToken)
        {
            var execution = await _sender.Send(new GetWorkflowExecutionByIdQuery(id), cancellationToken);

            if (execution is null)
                return NotFound();

            return Ok(execution);
        }
    }
}
