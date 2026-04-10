using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowForge.Persistence.Configurations
{
    internal class WorkflowTriggerConfiguration : IEntityTypeConfiguration<WorkflowTrigger>
    {
        public void Configure(EntityTypeBuilder<WorkflowTrigger> builder)
        {
            builder.ToTable("WorkflowTriggers");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.WorkflowId)
                .IsRequired();

            builder.Property(t => t.EventName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.SourceSystem)
                .IsRequired()
                .HasMaxLength(100);

            // Composite index for trigger resolution queries (EventName + SourceSystem lookups)
            builder.HasIndex(t => new { t.EventName, t.SourceSystem })
                .HasDatabaseName("IX_WorkflowTriggers_EventName_SourceSystem");

            builder.HasIndex(t => t.WorkflowId)
                .HasDatabaseName("IX_WorkflowTriggers_WorkflowId");
        }
    }
}
