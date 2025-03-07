using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddPresentation();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();