using FlowForge.Application.WorkflowEngine.Actions;
using FlowForge.Application.WorkflowEngine.Context;
using FlowForge.Domain.Entities;
using FlowForge.Persistence;
using Microsoft.Extensions.Logging;

namespace FlowForge.Application.WorkflowEngine.Steps
{
    public class StepExecutor : IStepExecutor
    {
        private readonly FlowForgeDbContext _db;
        private readonly ILogger<StepExecutor> _logger;

        // Keyed by ActionType string — built once from all registered IActionHandler implementations
        private readonly Dictionary<string, IActionHandler> _handlers;

        public StepExecutor(
            FlowForgeDbContext db,
            IEnumerable<IActionHandler> handlers,
            ILogger<StepExecutor> logger)
        {
            _db = db;
            _logger = logger;
            _handlers = handlers.ToDictionary(h => h.ActionType, StringComparer.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(WorkflowStep step, WorkflowExecutionContext context)
        {
            var actionType = step.ActionDefinition?.ActionType ?? "Unknown";

            var actionExecution = ActionExecution.Create(context.ExecutionId, step.Id);
            _db.ActionExecutions.Add(actionExecution);
            actionExecution.MarkRunning();
            await _db.SaveChangesAsync();

            _logger.LogInformation(
                "[FlowForge] Step Order={Order} ActionType={ActionType} | Starting",
                step.Order, actionType);

            try
            {
                if (!_handlers.TryGetValue(actionType, out var handler))
                {
                    throw new InvalidOperationException(
                        $"No action handler registered for ActionType '{actionType}'. Check that the handler is registered in DI.");
                }

                await handler.ExecuteAsync(step.ConfigurationJson, context);

                actionExecution.MarkCompleted();
                _logger.LogInformation(
                    "[FlowForge] Step Order={Order} ActionType={ActionType} | Completed",
                    step.Order, actionType);
            }
            catch (Exception ex)
            {
                actionExecution.MarkFailed();
                _logger.LogError(ex,
                    "[FlowForge] Step Order={Order} ActionType={ActionType} | Failed",
                    step.Order, actionType);

                throw new InvalidOperationException(
                    $"Step {step.Order} (ActionType='{actionType}') failed: {ex.Message}", ex);
            }
            finally
            {
                await _db.SaveChangesAsync();
            }
        }
    }
}
