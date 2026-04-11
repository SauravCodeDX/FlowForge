using FlowForge.Application.Workflows;
using FlowForge.Application.Workflows.Commands.CreateWorkflow;
using FlowForge.Application.Workflows.Commands.DeleteWorkflow;
using FlowForge.Application.Workflows.Queries.GetAllWorkflows;
using FlowForge.Application.Workflows.Queries.GetWorkflowById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly ISender _sender;

        public WorkflowsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<List<WorkflowDto>>> GetAllWorkflows(CancellationToken cancellationToken)
        {
            var workflows = await _sender.Send(new GetAllWorkflowsQuery(), cancellationToken);
            return Ok(workflows);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WorkflowDto>> GetWorkflowById(Guid id, CancellationToken cancellationToken)
        {
            var workflow = await _sender.Send(new GetWorkflowByIdQuery(id), cancellationToken);

            if (workflow is null)
                return NotFound();

            return Ok(workflow);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateWorkflow([FromBody] CreateWorkflowCommand command, CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetWorkflowById), new { id }, id);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWorkflow(Guid id, CancellationToken cancellationToken)
        {
            await _sender.Send(new DeleteWorkflowCommand(id), cancellationToken);
            return NoContent();
        }
    }
}
