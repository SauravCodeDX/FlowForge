using MediatR;

namespace FlowForge.Application.WorkflowExecutions.Queries.GetWorkflowExecutionById
{
    public record GetWorkflowExecutionByIdQuery(Guid Id) : IRequest<WorkflowExecutionDto?>;
}
