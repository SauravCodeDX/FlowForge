using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowForge.Domain.Entities
{
    //Step1 → Slack message sent
    //Step2 → Email sent
    public class ActionExecution
    {
        public Guid Id { get; internal set; }

        public Guid WorkflowExecutionId { get; internal set; }

        public Guid WorkflowStepId { get; internal set; }

        public string Status { get; internal set; } = default!;

        public DateTime ExecutedAt { get; internal set; }

        // Navigation properties
        public WorkflowExecution? WorkflowExecution { get; internal set; }

        public WorkflowStep? WorkflowStep { get; internal set; }
    }
}
