using Authentication.Application;
using Authentication.Infrastructure;
using AuthenticationApiService.Errors;
using AuthenticationApiService.Mapping;
using JwtAuthenticationManager;
using MessageBus;
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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
