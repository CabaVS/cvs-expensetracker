using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#if ASPIRE
builder.AddServiceDefaults();
#endif

builder.Services.AddApplication();
builder.Services.AddPersistence();
builder.Services.AddPresentation();

WebApplication app = builder.Build();

#if ASPIRE
app.MapDefaultEndpoints();
#endif

app.MapGet("/", () => "Hello World!");

await app.RunAsync();
