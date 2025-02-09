namespace CabaVS.ExpenseTracker.Application.Features.Reports.Models;

public sealed record IncomeExpenseReportModel(
    IncomeExpenseReportModel.ReportLine[] Incomes,
    IncomeExpenseReportModel.ReportLine[] Expenses,
    IncomeExpenseReportModel.ReportLine[] Diff)
{
    public sealed record ReportLine(decimal Amount, string CurrencyCode); 
}
