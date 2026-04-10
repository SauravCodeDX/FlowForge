using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    //EmployeeCreated → Workflow executed
    public class WorkflowExecution
    {
        public Guid Id { get; internal set; }

        public Guid WorkflowId { get; internal set; }

        public DateTime StartedAt { get; internal set; }

        public string Status { get; internal set; } = default!;

        // Navigation property
        public Workflow? Workflow { get; internal set; }

        public ICollection<ActionExecution>? ActionExecutions { get; internal set; }
    }
}
