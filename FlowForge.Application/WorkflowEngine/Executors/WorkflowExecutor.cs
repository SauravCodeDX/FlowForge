using System.Text.Json;
using FlowForge.Application.WorkflowEngine.Context;
using FlowForge.Application.WorkflowEngine.Steps;
using FlowForge.Application.WorkflowEngine.Triggers;
using FlowForge.Domain.Entities;
using FlowForge.Persistence;
using Microsoft.Extensions.Logging;

namespace FlowForge.Application.WorkflowEngine.Executors
{
    public class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly ITriggerResolver _triggerResolver;
        private readonly IStepExecutor _stepExecutor;
        private readonly FlowForgeDbContext _db;
        private readonly ILogger<WorkflowExecutor> _logger;

        public WorkflowExecutor(
            ITriggerResolver triggerResolver,
            IStepExecutor stepExecutor,
            FlowForgeDbContext db,
            ILogger<WorkflowExecutor> logger)
        {
            _triggerResolver = triggerResolver;
            _stepExecutor = stepExecutor;
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Entry point called by the API event controller.
        /// 1. Resolves matching workflows for the incoming event.
        /// 2. For each workflow, creates a WorkflowExecution record.
        /// 3. Runs steps sequentially; marks execution completed or failed.
        /// </summary>
        public async Task ExecuteAsync(string eventName, string sourceSystem, string payload)
        {
            _logger.LogInformation(
                "[FlowForge] Event received: {EventName} from {SourceSystem}",
                eventName, sourceSystem);

            var workflows = await _triggerResolver.ResolveAsync(eventName, sourceSystem);

            if (workflows.Count == 0)
            {
                _logger.LogWarning(
                    "[FlowForge] No active workflows found for event={EventName} source={SourceSystem}",
                    eventName, sourceSystem);
                return;
            }

            foreach (var workflow in workflows)
            {
                await RunWorkflowAsync(workflow, eventName, sourceSystem, payload);
            }
        }

        private async Task RunWorkflowAsync(
            Workflow workflow,
            string eventName,
            string sourceSystem,
            string payload)
        {
            _logger.LogInformation(
                "[FlowForge] Starting workflow '{WorkflowName}' ({WorkflowId})",
                workflow.Name, workflow.Id);

            var execution = WorkflowExecution.Create(workflow.Id);
            _db.WorkflowExecutions.Add(execution);
            await _db.SaveChangesAsync();

            var context = new WorkflowExecutionContext
            {
                WorkflowId = workflow.Id,
                ExecutionId = execution.Id,
                EventName = eventName,
                SourceSystem = sourceSystem,
                Payload = DeserializePayload(payload)
            };

            try
            {
                var steps = (workflow.Steps ?? Enumerable.Empty<Domain.Entities.WorkflowStep>())
                    .OrderBy(s => s.Order)
                    .ToList();

                _logger.LogInformation(
                    "[FlowForge] Workflow '{WorkflowName}' has {StepCount} step(s)",
                    workflow.Name, steps.Count);

                foreach (var step in steps)
                {
                    await _stepExecutor.ExecuteAsync(step, context);
                }

                execution.MarkCompleted();
                _logger.LogInformation(
                    "[FlowForge] Workflow '{WorkflowName}' execution {ExecutionId} COMPLETED",
                    workflow.Name, execution.Id);
            }
            catch (Exception ex)
            {
                execution.MarkFailed();
                _logger.LogError(ex,
                    "[FlowForge] Workflow '{WorkflowName}' execution {ExecutionId} FAILED",
                    workflow.Name, execution.Id);
            }
            finally
            {
                await _db.SaveChangesAsync();
            }
        }

        private static Dictionary<string, object> DeserializePayload(string payload)
        {
            if (string.IsNullOrWhiteSpace(payload)) return new();

            try
            {
                return JsonSerializer.Deserialize<Dictionary<string, object>>(payload) ?? new();
            }
            catch
            {
                // If payload isn't valid JSON, wrap it as a single "raw" field
                return new Dictionary<string, object> { ["raw"] = payload };
            }
        }
    }
}
