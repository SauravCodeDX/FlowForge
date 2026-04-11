namespace FlowForge.Domain.Entities
{
    //Step1 → Slack message sent
    //Step2 → Email sent
    public class ActionExecution
    {
        private ActionExecution() { } // Required by EF Core

        public Guid Id { get; private set; }

        public Guid WorkflowExecutionId { get; private set; }

        public Guid WorkflowStepId { get; private set; }

        public string Status { get; private set; } = default!;

        public DateTime ExecutedAt { get; private set; }

        // Navigation properties
        public WorkflowExecution? WorkflowExecution { get; private set; }

        public WorkflowStep? WorkflowStep { get; private set; }

        public static ActionExecution Create(Guid workflowExecutionId, Guid workflowStepId)
        {
            return new ActionExecution
            {
                Id = Guid.NewGuid(),
                WorkflowExecutionId = workflowExecutionId,
                WorkflowStepId = workflowStepId,
                Status = "Pending",
                ExecutedAt = DateTime.UtcNow
            };
        }

        public void MarkRunning() => Status = "Running";
        public void MarkCompleted() => Status = "Success";
        public void MarkFailed() => Status = "Failed";
    }
}
