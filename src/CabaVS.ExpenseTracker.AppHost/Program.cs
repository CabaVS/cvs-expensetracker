IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.CabaVS_ExpenseTracker_API>("cvs-expensetracker-api");

await builder.Build().RunAsync();
