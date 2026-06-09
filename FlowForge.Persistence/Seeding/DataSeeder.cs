using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlowForge.Persistence.Seeding
{
    public class DataSeeder
    {
        private readonly FlowForgeDbContext _db;
        private readonly ILogger<DataSeeder> _logger;

        public DataSeeder(FlowForgeDbContext db, ILogger<DataSeeder> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            // Only seed if ActionDefinitions table is empty
            if (await _db.ActionDefinitions.AnyAsync())
            {
                _logger.LogInformation("[Seeder] Data already exists, skipping.");
                return;
            }

            _logger.LogInformation("[Seeder] Seeding initial data...");

            // 1. Action Definitions
            var logAction = new ActionDefinition { Id = Guid.NewGuid(), Name = "Log Message", ActionType = "Log" };
            var httpAction = new ActionDefinition { Id = Guid.NewGuid(), Name = "HTTP Call", ActionType = "HttpCall" };

            _db.ActionDefinitions.AddRange(logAction, httpAction);

            // 2. Workflow
            var workflow = Workflow.Create("HR Onboarding Workflow");
            _db.Workflows.Add(workflow);

            // 3. Trigger
            var trigger = new WorkflowTrigger
            {
                Id = Guid.NewGuid(),
                WorkflowId = workflow.Id,
                EventName = "EmployeeCreated",
                SourceSystem = "HR"
            };
            _db.WorkflowTriggers.Add(trigger);

            // 4. Step
            var step = new WorkflowStep
            {
                Id = Guid.NewGuid(),
                WorkflowId = workflow.Id,
                Order = 1,
                ActionDefinitionId = logAction.Id,
                ConfigurationJson = """{"message": "Welcome {{payload.name}}! Event from {{sourceSystem}}"}"""
            };
            _db.WorkflowSteps.Add(step);

            await _db.SaveChangesAsync();

            _logger.LogInformation("[Seeder] Done.");
        }
    }
}
