using FlowForge.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Application.ActionDefinitions.Queries.GetAllActionDefinitions
{
    internal class GetAllActionDefinitionsQueryHandler : IRequestHandler<GetAllActionDefinitionsQuery, List<ActionDefinitionDto>>
    {
        private readonly FlowForgeDbContext _context;

        public GetAllActionDefinitionsQueryHandler(FlowForgeDbContext context)
        {
            _context = context;
        }

        public async Task<List<ActionDefinitionDto>> Handle(GetAllActionDefinitionsQuery request, CancellationToken cancellationToken)
        {
            return await _context.ActionDefinitions
                .Select(a => new ActionDefinitionDto(a.Id, a.Name, a.ActionType))
                .ToListAsync(cancellationToken);
        }
    }
}
