using Authentication.Application;
using Authentication.Infrastructure;
using AuthenticationApiService.Errors;
using AuthenticationApiService.Mapping;
using HealthChecks.UI.Client;
using JwtAuthenticationManager;
using MessageBus;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddSingleton<ProblemDetailsFactory, AuthProblemDetailsFactory>();

// My DI
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMappings();
builder.Services.AddMessageBus();

builder.Services.AddJwtAuthentication();
builder.Services.AddJwtManagerDependency();

// Health checks
builder.Services
    .AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("AuthDatabase"));
builder.Services.AddHealthChecksUI(setupSettings: setup =>
{
    setup.SetEvaluationTimeInSeconds(15);
    setup.AddHealthCheckEndpoint("auth-service", "/healthz"); //map health check api
}).AddInMemoryStorage();

var app = builder.Build();

// Health Checks
app.UseRouting().UseEndpoints(config =>
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
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
