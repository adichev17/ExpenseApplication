using AuthenticationApiService.Errors;
using AuthenticationApiService.Services;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<JwtTokenHandler>();
builder.Services.AddSingleton<ProblemDetailsFactory, AuthProblemDetailsFactory>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
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
