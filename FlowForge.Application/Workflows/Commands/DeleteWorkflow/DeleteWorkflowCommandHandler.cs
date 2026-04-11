using FlowForge.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Application.Workflows.Commands.DeleteWorkflow
{
    internal class DeleteWorkflowCommandHandler : IRequestHandler<DeleteWorkflowCommand>
    {
        private readonly FlowForgeDbContext _context;

        public DeleteWorkflowCommandHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteWorkflowCommand request, CancellationToken cancellationToken)
        {
            var workflow = await _context.Workflows
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workflow is null)
                return;

            _context.Workflows.Remove(workflow);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
