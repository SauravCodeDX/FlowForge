namespace FlowForge.Domain.Entities
{
    //EmployeeCreated → Workflow executed
    public class WorkflowExecution
    {
        private WorkflowExecution() { } // Required by EF Core

        public Guid Id { get; private set; }

        public Guid WorkflowId { get; private set; }

        public DateTime StartedAt { get; private set; }

        public string Status { get; private set; } = default!;

        // Navigation properties
        public Workflow? Workflow { get; private set; }

        public ICollection<ActionExecution>? ActionExecutions { get; private set; }

        public static WorkflowExecution Create(Guid workflowId)
        {
            return new WorkflowExecution
            {
                Id = Guid.NewGuid(),
                WorkflowId = workflowId,
                StartedAt = DateTime.UtcNow,
                Status = "Running"
            };
        }

        public void MarkCompleted() => Status = "Success";
        public void MarkFailed() => Status = "Failed";
    }
}
