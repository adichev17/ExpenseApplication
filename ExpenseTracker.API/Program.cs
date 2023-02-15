using ExpenseTracker.API.Common.ErrorHandler;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddSingleton<ProblemDetailsFactory, ExpenseTrackerProblemDetailsFactory>();

//DI
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseExceptionHandler("/error");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
