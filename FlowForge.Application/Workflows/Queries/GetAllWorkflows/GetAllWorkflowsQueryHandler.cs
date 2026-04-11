using FlowForge.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Application.Workflows.Queries.GetAllWorkflows
{
    internal class GetAllWorkflowsQueryHandler : IRequestHandler<GetAllWorkflowsQuery, List<WorkflowDto>>
    {
        private readonly FlowForgeDbContext _context;

        public GetAllWorkflowsQueryHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkflowDto>> Handle(GetAllWorkflowsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Workflows
                .Select(w => new WorkflowDto(w.Id, w.Name, w.IsActive))
                .ToListAsync(cancellationToken);
        }
    }
}
