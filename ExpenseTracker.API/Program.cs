using ExpenseTracker.API.Common.ErrorHandler;
using ExpenseTracker.API.Common.Mapping;
using ExpenseTracker.API.Messaging;
using ExpenseTracker.Application;
using ExpenseTracker.Infrastructure;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<ProblemDetailsFactory, ExpenseTrackerProblemDetailsFactory>();

builder.Services.AddJwtAuthentication();

//DI
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddMappings();

//BW
//builder.Services.AddHostedService<RabbitMQConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
