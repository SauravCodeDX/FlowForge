using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowForge.Persistence.Configurations
{
    internal class ActionExecutionConfiguration : IEntityTypeConfiguration<ActionExecution>
    {
        public void Configure(EntityTypeBuilder<ActionExecution> builder)
        {
            builder.ToTable("ActionExecutions");

            builder.HasKey(ae => ae.Id);
            builder.Property(ae => ae.Id)
                .ValueGeneratedNever();

            builder.Property(ae => ae.WorkflowExecutionId)
                .IsRequired();

            builder.Property(ae => ae.WorkflowStepId)
                .IsRequired();

            builder.Property(ae => ae.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(ae => ae.ExecutedAt)
                .IsRequired();

            // Restrict so that deleting a step does not silently wipe execution history
            builder.HasOne(ae => ae.WorkflowStep)
                .WithMany()
                .HasForeignKey(ae => ae.WorkflowStepId)
                .OnDelete(DeleteBehavior.Restrict);

            // Support common queries: all action executions for a run, and status filtering
            builder.HasIndex(ae => ae.WorkflowExecutionId)
                .HasDatabaseName("IX_ActionExecutions_WorkflowExecutionId");

            builder.HasIndex(ae => ae.WorkflowStepId)
                .HasDatabaseName("IX_ActionExecutions_WorkflowStepId");

            builder.HasIndex(ae => ae.Status)
                .HasDatabaseName("IX_ActionExecutions_Status");
        }
    }
}
