using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Application.WorkflowEngine.Context
{
    public class WorkflowExecutionContext
    {
        public Guid WorkflowId { get; set; }

        public Guid ExecutionId { get; set; }

        public string EventName { get; set; } = default!;

        public string SourceSystem { get; set; } = default!;

        public Dictionary<string, object> Payload { get; set; } = new();

        public Dictionary<string, object> Variables { get; set; } = new();
    }
}
