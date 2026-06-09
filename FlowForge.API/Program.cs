using FlowForge.Application.WorkflowEngine.Actions;
using FlowForge.Persistence.Seeding;
using FlowForge.Application.WorkflowEngine.Executors;
using FlowForge.Application.WorkflowEngine.Steps;
using FlowForge.Application.WorkflowEngine.Triggers;
using FlowForge.Application.Workflows.Commands.CreateWorkflow;
using FlowForge.Infrastructure.Actions;
using FlowForge.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── Local secrets override (gitignored — never committed) ────────────────────
// Loads appsettings.Local.json if present (local dev secrets like API keys).
// In Staging/Production, secrets come from environment variables or Key Vault.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

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

// ── Startup: migrate DB + seed data ─────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlowForgeDbContext>();
    var startupLogger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    startupLogger.LogInformation("[FlowForge] Database migration started...");
    await db.Database.MigrateAsync();
    startupLogger.LogInformation("[FlowForge] Database migration finished successfully.");

    var seeder = new DataSeeder(
        db,
        scope.ServiceProvider.GetRequiredService<ILogger<DataSeeder>>());
    await seeder.SeedAsync();
    startupLogger.LogInformation("[FlowForge] Startup complete. FlowForge is ready.");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
