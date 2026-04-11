using FlowForge.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Application.WorkflowExecutions.Queries.GetWorkflowExecutionById
{
    internal class GetWorkflowExecutionByIdQueryHandler : IRequestHandler<GetWorkflowExecutionByIdQuery, WorkflowExecutionDto?>
    {
        private readonly FlowForgeDbContext _context;

        public GetWorkflowExecutionByIdQueryHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task<WorkflowExecutionDto?> Handle(GetWorkflowExecutionByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.WorkflowExecutions
                .Where(e => e.Id == request.Id)
                .Select(e => new WorkflowExecutionDto(e.Id, e.WorkflowId, e.StartedAt, e.Status))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
