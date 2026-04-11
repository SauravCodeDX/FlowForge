using FlowForge.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Application.Workflows.Queries.GetWorkflowById
{
    internal class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, WorkflowDto?>
    {
        private readonly FlowForgeDbContext _context;

        public GetWorkflowByIdQueryHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowDto?> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Workflows
                .Where(w => w.Id == request.Id)
                .Select(w => new WorkflowDto(w.Id, w.Name, w.IsActive))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
