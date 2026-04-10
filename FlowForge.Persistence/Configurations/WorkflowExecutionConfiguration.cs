using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowForge.Persistence.Configurations
{
    internal class WorkflowExecutionConfiguration : IEntityTypeConfiguration<WorkflowExecution>
    {
        public void Configure(EntityTypeBuilder<WorkflowExecution> builder)
        {
            builder.ToTable("WorkflowExecutions");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.WorkflowId)
                .IsRequired();

            builder.Property(e => e.StartedAt)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            // Restrict so execution history is preserved even if the workflow is deleted
            builder.HasOne(e => e.Workflow)
                .WithMany()
                .HasForeignKey(e => e.WorkflowId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ActionExecutions)
                .WithOne(ae => ae.WorkflowExecution)
                .HasForeignKey(ae => ae.WorkflowExecutionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Common query patterns: filter by workflow and filter by status
            builder.HasIndex(e => e.WorkflowId)
                .HasDatabaseName("IX_WorkflowExecutions_WorkflowId");

            builder.HasIndex(e => e.Status)
                .HasDatabaseName("IX_WorkflowExecutions_Status");
        }
    }
}
