using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    public class WorkflowStep
    {
        public Guid Id { get; internal set; }

        public Guid WorkflowId { get; internal set; }

        public int Order { get; internal set; }

        public Guid ActionDefinitionId { get; internal set; }

        public string ConfigurationJson { get; internal set; } = default!;

        // Navigation properties
        public Workflow? Workflow { get; internal set; }

        public ActionDefinition? ActionDefinition { get; internal set; }
    }
}
