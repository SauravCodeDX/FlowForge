using FlowForge.Domain.Entities;
using FlowForge.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlowForge.Application.WorkflowEngine.Triggers
{
    public class TriggerResolver : ITriggerResolver
    {
        private readonly FlowForgeDbContext _db;
        private readonly ILogger<TriggerResolver> _logger;

        public TriggerResolver(FlowForgeDbContext db, ILogger<TriggerResolver> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Returns all active workflows that have a trigger matching the
        /// given eventName + sourceSystem combination, with their steps
        /// and action definitions eagerly loaded.
        /// </summary>
        public async Task<List<Workflow>> ResolveAsync(string eventName, string sourceSystem)
        {
            var workflows = await _db.WorkflowTriggers
                .Where(t => t.EventName == eventName && t.SourceSystem == sourceSystem)
                .Include(t => t.Workflow)
                    .ThenInclude(w => w!.Steps!)
                        .ThenInclude(s => s.ActionDefinition)
                .Where(t => t.Workflow != null && t.Workflow.IsActive)
                .Select(t => t.Workflow!)
                .Distinct()
                .ToListAsync();

            _logger.LogInformation(
                "[FlowForge] TriggerResolver: event={EventName} source={SourceSystem} → {Count} workflow(s) matched",
                eventName, sourceSystem, workflows.Count);

            return workflows;
        }
    }
}
