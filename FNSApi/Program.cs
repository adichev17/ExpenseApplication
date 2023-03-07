using FNSApi.Common;
using FNSApi.Common.Mapping;
using FNSApi.Messaging;
using FNSApi.Services;
using FNSApi.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.Configure<FnsSettings>(builder.Configuration.GetSection(nameof(FnsSettings)));
builder.Services.AddSingleton<IFnsHttpService, FnsHttpService>();
builder.Services.AddHttpClient<IFnsHttpService, FnsHttpService>(client =>
{
    var fnsSettings = builder.Configuration.GetSection(nameof(FnsSettings)).Get<FnsSettings>();
    client.BaseAddress = new Uri(fnsSettings.BaseAddress);
    client.DefaultRequestHeaders.Add("Accept", "*/*");
    client.DefaultRequestHeaders.Add("Device-OS", "iOS");
    client.DefaultRequestHeaders.Add("clientVersion", "2.9.0");
    client.DefaultRequestHeaders.Add("Device-Id", "7C82010F-16CC-446B-8F66-FC4080C66521");
});

builder.Services.AddMappings();
builder.Services.AddMemoryCache();

builder.Services.AddHostedService<RabbitMQPhoneConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
