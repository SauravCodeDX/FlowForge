using FlowForge.Application.WorkflowEngine.Context;

namespace FlowForge.Application.WorkflowEngine.Actions
{
    /// <summary>
    /// Implemented by each concrete action type (Log, HttpCall, SendEmail, etc.).
    /// Register all implementations in DI; StepExecutor resolves them by ActionType.
    /// </summary>
    public interface IActionHandler
    {
        /// <summary>
        /// Must match the ActionDefinition.ActionType stored in the database.
        /// e.g. "Log", "HttpCall", "SendEmail"
        /// </summary>
        string ActionType { get; }

        Task ExecuteAsync(string configurationJson, WorkflowExecutionContext context);
    }
}
