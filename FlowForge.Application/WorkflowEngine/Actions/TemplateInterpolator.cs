using System.Text;
using System.Text.Json;
using FlowForge.Application.WorkflowEngine.Context;

namespace FlowForge.Application.WorkflowEngine.Actions
{
    /// <summary>
    /// Replaces {{tokens}} in action config strings with runtime values.
    ///
    /// Supported tokens:
    ///   {{eventName}}       — the incoming event name
    ///   {{sourceSystem}}    — the originating system
    ///   {{executionId}}     — current execution GUID
    ///   {{payload.field}}   — top-level field from the event payload JSON
    ///   {{variables.key}}   — value set by a previous step into context.Variables
    /// </summary>
    public static class TemplateInterpolator
    {
        public static string Interpolate(string template, WorkflowExecutionContext context)
        {
            if (string.IsNullOrEmpty(template)) return template;

            var sb = new StringBuilder(template);

            sb.Replace("{{eventName}}", context.EventName);
            sb.Replace("{{sourceSystem}}", context.SourceSystem);
            sb.Replace("{{executionId}}", context.ExecutionId.ToString());
            sb.Replace("{{workflowId}}", context.WorkflowId.ToString());

            foreach (var (key, value) in context.Payload)
            {
                var raw = value is JsonElement je ? je.ToString() : value?.ToString() ?? "";
                sb.Replace($"{{{{payload.{key}}}}}", raw);
            }

            foreach (var (key, value) in context.Variables)
            {
                sb.Replace($"{{{{variables.{key}}}}}", value?.ToString() ?? "");
            }

            return sb.ToString();
        }
    }
}
