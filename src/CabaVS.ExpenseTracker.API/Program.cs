using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

var isAspireMode = string.Equals(
    Environment.GetEnvironmentVariable("ASPIRE_MODE"),
    true.ToString(),
    StringComparison.OrdinalIgnoreCase);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (isAspireMode)
{
    builder.AddServiceDefaults();
}

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddPresentation();

WebApplication app = builder.Build();

if (isAspireMode)
{
    app.MapDefaultEndpoints();
}

app.MapGet("/", () => "Hello World!");

await app.RunAsync();
