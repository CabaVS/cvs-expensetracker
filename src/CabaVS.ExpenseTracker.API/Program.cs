using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Infrastructure;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddPresentation(builder.Configuration, builder.Environment);

WebApplication app = builder.Build();

app.UsePresentation();

await app.RunAsync();
