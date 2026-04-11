using MediatR;

namespace FlowForge.Application.Workflows.Commands.DeleteWorkflow
{
    public record DeleteWorkflowCommand(Guid Id) : IRequest;
}
