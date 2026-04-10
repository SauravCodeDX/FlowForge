using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    //Shopify Order Created
    //HR Employee Created
    //GitHub PR Opened
    public class WorkflowTrigger
    {
        public Guid Id { get; internal set; }

        public Guid WorkflowId { get; internal set; }

        public string EventName { get; internal set; } = default!;

        public string SourceSystem { get; internal set; } = default!;

        // Navigation property
        public Workflow? Workflow { get; internal set; }
    }
}
