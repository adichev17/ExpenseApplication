using ExpenseTracker.API.Common.ErrorHandler;
using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Messaging;
using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;
using HealthChecks.UI.Client;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<ProblemDetailsFactory, ExpenseTrackerProblemDetailsFactory>();

builder.Services.AddJwtAuthentication();

//DI
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMappings();

var databaseConnectionString = builder.Configuration.GetConnectionString("ExpenseTrackerDatabase");
var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");

// Health checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(databaseConnectionString, name: "Database", failureStatus: HealthStatus.Degraded,
        timeout: TimeSpan.FromSeconds(1), tags: new string[] { "services" })
    .AddRabbitMQ(rabbitMqConnectionString, name: "RabbitMQ", failureStatus: HealthStatus.Degraded,
        timeout: TimeSpan.FromSeconds(1), tags: new string[] { "services" });
builder.Services.AddHealthChecksUI(setupSettings: setup =>
{
    setup.SetEvaluationTimeInSeconds(5);
    setup.AddHealthCheckEndpoint("auth-service", "/healthz"); //map health check api
}).AddInMemoryStorage();

//BW
builder.Services.AddHostedService<RabbitMQConsumer>();

var app = builder.Build();

// Health Checks
app.UseRouting();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(config =>
{
    config.MapHealthChecks(
        "/healthz",
        new HealthCheckOptions
        {
            ResponseWriter =
                UIResponseWriter.WriteHealthCheckUIResponse
        });
    config.MapHealthChecksUI(x => x.UIPath = "/health-dashboard");
});
app.MapControllers();

app.Run();
