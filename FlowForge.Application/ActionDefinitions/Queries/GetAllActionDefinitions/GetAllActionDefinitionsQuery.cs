using MediatR;

namespace FlowForge.Application.ActionDefinitions.Queries.GetAllActionDefinitions
{
    public record GetAllActionDefinitionsQuery : IRequest<List<ActionDefinitionDto>>;
}
