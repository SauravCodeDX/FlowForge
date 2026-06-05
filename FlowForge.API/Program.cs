using FlowForge.Application.WorkflowEngine.Actions;
using FlowForge.Application.WorkflowEngine.Executors;
using FlowForge.Application.WorkflowEngine.Steps;
using FlowForge.Application.WorkflowEngine.Triggers;
using FlowForge.Application.Workflows.Commands.CreateWorkflow;
using FlowForge.Infrastructure.Actions;
using FlowForge.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Database ────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<FlowForgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── MediatR (CQRS) ──────────────────────────────────────────────────────────
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateWorkflowCommand).Assembly));

// ── Workflow Engine ──────────────────────────────────────────────────────────
builder.Services.AddScoped<ITriggerResolver, TriggerResolver>();
builder.Services.AddScoped<IStepExecutor, StepExecutor>();
builder.Services.AddScoped<IWorkflowExecutor, WorkflowExecutor>();

// ── Action Handlers ──────────────────────────────────────────────────────────
// Register each handler both as IActionHandler (for the collection in StepExecutor)
// and as its concrete type (so DI can construct it).
builder.Services.AddScoped<IActionHandler, LogActionHandler>();
builder.Services.AddScoped<IActionHandler, HttpCallActionHandler>();

// ── HTTP Client (used by HttpCallActionHandler) ──────────────────────────────
builder.Services.AddHttpClient("FlowForge.HttpCall");

// ── API ──────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "FlowForge API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
