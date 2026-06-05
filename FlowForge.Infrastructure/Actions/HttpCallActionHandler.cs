using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FlowForge.Application.WorkflowEngine.Actions;
using FlowForge.Application.WorkflowEngine.Context;
using Microsoft.Extensions.Logging;

namespace FlowForge.Infrastructure.Actions
{
    /// <summary>
    /// ActionType: "HttpCall"
    ///
    /// Config JSON example:
    /// {
    ///   "url":    "https://hooks.slack.com/services/...",
    ///   "method": "POST",
    ///   "headers": { "Content-Type": "application/json" },
    ///   "body":   "{\"text\": \"New employee: {{payload.name}}\"}"
    /// }
    ///
    /// All string values support {{token}} interpolation.
    /// </summary>
    public class HttpCallActionHandler : IActionHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpCallActionHandler> _logger;

        public HttpCallActionHandler(
            IHttpClientFactory httpClientFactory,
            ILogger<HttpCallActionHandler> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public string ActionType => "HttpCall";

        public async Task ExecuteAsync(string configurationJson, WorkflowExecutionContext context)
        {
            var config = JsonSerializer.Deserialize<HttpCallConfig>(configurationJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new InvalidOperationException("HttpCall action requires valid configuration JSON.");

            if (string.IsNullOrWhiteSpace(config.Url))
                throw new InvalidOperationException("HttpCall action requires a 'url' in configuration.");

            var url    = TemplateInterpolator.Interpolate(config.Url, context);
            var body   = TemplateInterpolator.Interpolate(config.Body ?? string.Empty, context);
            var method = new HttpMethod(config.Method?.ToUpperInvariant() ?? "POST");

            var client = _httpClientFactory.CreateClient("FlowForge.HttpCall");
            var request = new HttpRequestMessage(method, url);

            if (!string.IsNullOrEmpty(body))
            {
                var contentType = config.Headers?.GetValueOrDefault("Content-Type") ?? "application/json";
                request.Content = new StringContent(body, Encoding.UTF8, contentType);
            }

            if (config.Headers != null)
            {
                foreach (var (key, value) in config.Headers)
                {
                    if (key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase)) continue;
                    request.Headers.TryAddWithoutValidation(key, value);
                }
            }

            _logger.LogInformation(
                "[FlowForge] HttpCall {Method} {Url}", method, url);

            var response = await client.SendAsync(request);

            _logger.LogInformation(
                "[FlowForge] HttpCall response: {StatusCode}", (int)response.StatusCode);

            response.EnsureSuccessStatusCode();
        }

        private sealed class HttpCallConfig
        {
            public string Url { get; set; } = string.Empty;
            public string? Method { get; set; }
            public string? Body { get; set; }
            public Dictionary<string, string>? Headers { get; set; }
        }
    }
}
