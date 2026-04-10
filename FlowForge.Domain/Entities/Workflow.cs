using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    //Example - "New Employee Onboarding Workflow"
    public class Workflow
    {
        public Guid Id { get; internal set; }

        public string Name { get; internal set; } = default!;

        public bool IsActive { get; internal set; }

        public ICollection<WorkflowTrigger>? Triggers { get; internal set; }

        public ICollection<WorkflowStep>? Steps { get; internal set; }
    }
}
