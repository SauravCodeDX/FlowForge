using FlowForge.Application.ActionDefinitions;
using FlowForge.Application.ActionDefinitions.Queries.GetAllActionDefinitions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowForge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActionDefinitionsController : ControllerBase
    {
        private readonly ISender _sender;

        public ActionDefinitionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActionDefinitionDto>>> GetAllActionDefinitions(CancellationToken cancellationToken)
        {
            var definitions = await _sender.Send(new GetAllActionDefinitionsQuery(), cancellationToken);
            return Ok(definitions);
        }
    }
}
