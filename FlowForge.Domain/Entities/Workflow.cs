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
        private Workflow() { } // Required by EF Core

        public Guid Id { get; private set; }

        public string Name { get; private set; } = default!;

        public bool IsActive { get; private set; }

        public ICollection<WorkflowTrigger>? Triggers { get; private set; }

        public ICollection<WorkflowStep>? Steps { get; private set; }

        public static Workflow Create(string name)
        {
            return new Workflow
            {
                Id = Guid.NewGuid(),
                Name = name,
                IsActive = true
            };
        }
    }
}
