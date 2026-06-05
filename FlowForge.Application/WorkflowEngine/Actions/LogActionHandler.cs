using System.Text.Json;
using FlowForge.Application.WorkflowEngine.Context;
using Microsoft.Extensions.Logging;

namespace FlowForge.Application.WorkflowEngine.Actions
{
    /// <summary>
    /// ActionType: "Log"
    ///
    /// Config JSON example:
    ///   { "message": "Processing event {{eventName}} from {{sourceSystem}}" }
    ///
    /// Useful for debugging and as a no-op placeholder step.
    /// </summary>
    public class LogActionHandler : IActionHandler
    {
        private readonly ILogger<LogActionHandler> _logger;

        public LogActionHandler(ILogger<LogActionHandler> logger)
        {
            _logger = logger;
        }

        public string ActionType => "Log";

        public Task ExecuteAsync(string configurationJson, WorkflowExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<LogConfig>(configurationJson)
                         ?? new LogConfig { Message = "(no message configured)" };

            var message = TemplateInterpolator.Interpolate(config.Message, context);

            _logger.LogInformation(
                "[FlowForge] Workflow={WorkflowId} Execution={ExecutionId} | {Message}",
                context.WorkflowId, context.ExecutionId, message);

            return Task.CompletedTask;
        }

        private sealed class LogConfig
        {
            public string Message { get; set; } = string.Empty;
        }
    }
}
