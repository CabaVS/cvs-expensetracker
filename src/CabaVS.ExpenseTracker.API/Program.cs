using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddPresentation();

WebApplication app = builder.Build();

app.MapGet("/", () => "Hello World!");

await app.RunAsync();
