using FlowForge.Domain.Entities;
using FlowForge.Persistence;
using MediatR;

namespace FlowForge.Application.Workflows.Commands.CreateWorkflow
{
    internal class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, Guid>
    {
        private readonly FlowForgeDbContext _context;

        public CreateWorkflowCommandHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = Workflow.Create(request.Name);

            _context.Workflows.Add(workflow);
            await _context.SaveChangesAsync(cancellationToken);

            return workflow.Id;
        }
    }
}
