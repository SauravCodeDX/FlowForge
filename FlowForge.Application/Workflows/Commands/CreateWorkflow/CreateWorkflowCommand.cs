using MediatR;

namespace FlowForge.Application.Workflows.Commands.CreateWorkflow
{
    public record CreateWorkflowCommand(string Name) : IRequest<Guid>;
}
