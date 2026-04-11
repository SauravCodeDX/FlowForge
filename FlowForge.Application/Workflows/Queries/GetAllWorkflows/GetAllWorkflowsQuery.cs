using MediatR;

namespace FlowForge.Application.Workflows.Queries.GetAllWorkflows
{
    public record GetAllWorkflowsQuery : IRequest<List<WorkflowDto>>;
}
