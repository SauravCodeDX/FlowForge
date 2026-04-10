using FlowForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowForge.Persistence.Configurations
{
    internal class ActionDefinitionConfiguration : IEntityTypeConfiguration<ActionDefinition>
    {
        public void Configure(EntityTypeBuilder<ActionDefinition> builder)
        {
            builder.ToTable("ActionDefinitions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .ValueGeneratedNever();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);

            // ActionType is the registry key used to resolve the correct executor at runtime
            builder.Property(a => a.ActionType)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(a => a.ActionType)
                .IsUnique()
                .HasDatabaseName("IX_ActionDefinitions_ActionType");
        }
    }
}
