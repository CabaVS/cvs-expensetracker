namespace CabaVS.ExpenseTracker.Application.Features.Reports.Models;

public sealed record ByCategoryReportModel(
    Guid CategoryId,
    string CategoryName,
    decimal TotalAmount,
    string CurrencyCode);
