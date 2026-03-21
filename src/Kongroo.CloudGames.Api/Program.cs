using System.Globalization;
using HealthChecks.UI.Client;
using Kongroo.CloudGames.Api;
using Kongroo.CloudGames.Identity;
using Kongroo.CloudGames.Identity.Infrastructure;
using Kongroo.CloudGames.Identity.Presentation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddProblemDetails();
builder
    .Services.AddHealthChecks()
    .AddApplicationLifecycleHealthCheck()
    .AddResourceUtilizationHealthCheck()
    .AddNpgSql(builder.Configuration.GetRequiredConnectionString("Database"))
    .AddDbContextCheck<IdentityDbContext>();

builder.Services.AddSerilog(configuration =>
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithMachineName()
        .Enrich.WithProcessId()
        .Enrich.WithProcessName()
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .Enrich.WithProperty("Application", AppDomain.CurrentDomain.FriendlyName)
);

builder.Services.AddIdentityModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();

app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapIdentityEndpoints();

await app.RunAsync();
