using CabaVS.ExpenseTracker.Application;
using CabaVS.ExpenseTracker.Persistence;
using CabaVS.ExpenseTracker.Presentation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services
    .AddApplication()
    .AddPersistence(configuration)
    .AddPresentation(configuration, environment);

builder.WebHost
    .UseKestrel(options => options.AddServerHeader = false);

WebApplication app = builder.Build();

app.UsePresentationWithFastEndpoints();

await app.RunAsync();

// Required by xUnit public tests
#pragma warning disable CA1515
#pragma warning disable S1118
public partial class Program;
#pragma warning restore S1118
#pragma warning restore CA1515
