using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Reports.Models;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Reports.Queries;

public sealed record GetIncomeExpenseReportByDateQuery(Guid WorkspaceId, DateOnly StartDate, DateOnly EndDate) 
    : IRequest<Result<IncomeExpenseReportModel>>, IAuthenticatedUserRequest, IWorkspaceBoundRequest;

internal sealed class GetIncomeExpenseReportByDateQueryHandler(ITransactionQueryRepository transactionQueryRepository)
    : IRequestHandler<GetIncomeExpenseReportByDateQuery, Result<IncomeExpenseReportModel>>
{
    public async Task<Result<IncomeExpenseReportModel>> Handle(
        GetIncomeExpenseReportByDateQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return TimePeriodErrors.StartDateIsGreaterThanEndDate();
        }
        
        TransactionMoneyModel[] incomesData = await transactionQueryRepository.GetTransactionsMoneyAsync(
            request.WorkspaceId, TransactionType.Income, request.StartDate, request.EndDate, cancellationToken);
        TransactionMoneyModel[] expensesData = await transactionQueryRepository.GetTransactionsMoneyAsync(
            request.WorkspaceId, TransactionType.Expense, request.StartDate, request.EndDate, cancellationToken);
        
        IncomeExpenseReportModel.ReportLine[] incomes = incomesData
            .GroupBy(x => x.CurrencyCode)
            .Select(g => new IncomeExpenseReportModel.ReportLine(
                g.Sum(x => x.Amount),
                g.Key))
            .ToArray();
        IncomeExpenseReportModel.ReportLine[] expenses = expensesData
            .GroupBy(x => x.CurrencyCode)
            .Select(g => new IncomeExpenseReportModel.ReportLine(
                -g.Sum(x => x.Amount),
                g.Key))
            .ToArray();
        IncomeExpenseReportModel.ReportLine[] diff = incomes.Select(x => x.CurrencyCode)
            .Union(expenses.Select(x => x.CurrencyCode))
            .Select(x => new IncomeExpenseReportModel.ReportLine(
                (incomes.FirstOrDefault(y => y.CurrencyCode == x)?.Amount ?? 0) + 
                (expenses.FirstOrDefault(y => y.CurrencyCode == x)?.Amount ?? 0),
                x))
            .ToArray();

        var report = new IncomeExpenseReportModel(incomes, expenses, diff);
        return report;
    }
}
