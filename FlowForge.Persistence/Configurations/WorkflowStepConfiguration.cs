using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowForge.Persistence.Configurations
{
    internal class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
    {
        public void Configure(EntityTypeBuilder<WorkflowStep> builder)
        {
            builder.ToTable("WorkflowSteps");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .ValueGeneratedNever();

            builder.Property(s => s.WorkflowId)
                .IsRequired();

            builder.Property(s => s.Order)
                .IsRequired();

            builder.Property(s => s.ActionDefinitionId)
                .IsRequired();

            builder.Property(s => s.ConfigurationJson)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.HasIndex(s => new { s.WorkflowId, s.Order })
                .IsUnique()
                .HasDatabaseName("IX_WorkflowSteps_WorkflowId_Order");

            builder.HasIndex(s => s.ActionDefinitionId)
                .HasDatabaseName("IX_WorkflowSteps_ActionDefinitionId");

            builder.HasOne(s => s.ActionDefinition)
                .WithMany()
                .HasForeignKey(s => s.ActionDefinitionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
