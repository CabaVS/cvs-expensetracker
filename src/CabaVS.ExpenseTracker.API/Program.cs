using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services
    .AddApplication()
    .AddPersistence(configuration)
    .AddPresentation(environment);

builder.WebHost
    .UseKestrel(options => options.AddServerHeader = false);

var app = builder.Build();

app.UsePresentationWithFastEndpoints();

app.Run();