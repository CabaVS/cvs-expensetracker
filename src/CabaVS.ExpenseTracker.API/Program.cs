using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

builder.Services
    .AddApplication()
    .AddPersistence()
    .AddPresentation(configuration, environment);

var app = builder.Build();

app.UsePresentation();

app.Run();