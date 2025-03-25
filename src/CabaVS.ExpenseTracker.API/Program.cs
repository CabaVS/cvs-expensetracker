using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration, builder.Environment);

WebApplication app = builder.Build();

app.UsePresentation();

await app.RunAsync();
