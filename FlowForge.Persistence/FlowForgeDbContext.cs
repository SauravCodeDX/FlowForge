using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowForge.Persistence
{
    public class FlowForgeDbContext : DbContext
    {
        public FlowForgeDbContext(DbContextOptions<FlowForgeDbContext> options) : base(options)
        {
        }

        public DbSet<Workflow> Workflows => Set<Workflow>();
        public DbSet<WorkflowTrigger> WorkflowTriggers => Set<WorkflowTrigger>();
        public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();
        public DbSet<ActionDefinition> ActionDefinitions => Set<ActionDefinition>();
        public DbSet<WorkflowExecution> WorkflowExecutions => Set<WorkflowExecution>();
        public DbSet<ActionExecution> ActionExecutions => Set<ActionExecution>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlowForgeDbContext).Assembly);
        }
    }
}
