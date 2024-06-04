using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Infrastructure;
using CabaVS.ExpenseTracker.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddPresentation(environment);

builder.Host
    .UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration));

builder.WebHost
    .UseKestrel(options => options.AddServerHeader = false);

var app = builder.Build();

app.UseFastEndpoints();
app.UseSerilogRequestLogging();

app.Run();