using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

#if ASPIRE
builder.AddServiceDefaults();
#endif

builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);
builder.Services.AddPresentation(environment);

WebApplication app = builder.Build();

#if ASPIRE
app.MapDefaultEndpoints();
#endif

app.UsePresentation();

await app.RunAsync();
