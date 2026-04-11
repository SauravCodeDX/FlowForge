using MediatR;

namespace FlowForge.Application.Workflows.Queries.GetWorkflowById
{
    public record GetWorkflowByIdQuery(Guid Id) : IRequest<WorkflowDto?>;
}
