using FlowForge.Application.WorkflowEngine.Executors;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FlowForge.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IWorkflowExecutor _workflowExecutor;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            IWorkflowExecutor workflowExecutor,
            ILogger<EventsController> logger)
        {
            _workflowExecutor = workflowExecutor;
            _logger = logger;
        }

        /// <summary>
        /// Intake endpoint for external systems to trigger FlowForge workflows.
        ///
        /// POST /api/events
        /// {
        ///   "eventName":    "EmployeeCreated",
        ///   "sourceSystem": "HR",
        ///   "payload": {
        ///     "name":  "Jane Doe",
        ///     "email": "jane@acme.com"
        ///   }
        /// }
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TriggerEvent([FromBody] TriggerEventRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EventName))
                return BadRequest("eventName is required.");

            if (string.IsNullOrWhiteSpace(request.SourceSystem))
                return BadRequest("sourceSystem is required.");

            // Serialize the payload dict back to JSON so the engine can work with it uniformly
            var payloadJson = request.Payload != null
                ? JsonSerializer.Serialize(request.Payload)
                : "{}";

            _logger.LogInformation(
                "Event intake: {EventName} from {SourceSystem}",
                request.EventName, request.SourceSystem);

            // Phase 1: execute synchronously (Worker service will make this async later)
            await _workflowExecutor.ExecuteAsync(request.EventName, request.SourceSystem, payloadJson);

            return Accepted(new
            {
                message = "Event received and processed.",
                eventName = request.EventName,
                sourceSystem = request.SourceSystem
            });
        }
    }

    public sealed class TriggerEventRequest
    {
        public string EventName { get; set; } = string.Empty;
        public string SourceSystem { get; set; } = string.Empty;

        /// <summary>
        /// Free-form JSON payload from the external system.
        /// Fields can be referenced in action configs via {{payload.fieldName}}.
        /// </summary>
        public Dictionary<string, object>? Payload { get; set; }
    }
}
