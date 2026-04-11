namespace FlowForge.Application.WorkflowExecutions
{
    public record WorkflowExecutionDto(Guid Id, Guid WorkflowId, DateTime StartedAt, string Status);
}
