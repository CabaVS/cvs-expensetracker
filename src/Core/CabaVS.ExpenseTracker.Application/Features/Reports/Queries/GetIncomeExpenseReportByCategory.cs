using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Transactions.Models;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Reports.Queries;

public sealed record GetIncomeExpenseReportByCategoryQuery(Guid WorkspaceId, DateOnly StartDate, DateOnly EndDate)
    : IRequest<Result<TransactionMoneyByCategoryModel[]>>, IAuthenticatedUserRequest, IWorkspaceBoundRequest;

internal sealed class GetIncomeExpenseReportByCategoryQueryHandler(
    ITransactionQueryRepository transactionQueryRepository)
    : IRequestHandler<GetIncomeExpenseReportByCategoryQuery, Result<TransactionMoneyByCategoryModel[]>>
{
    public async Task<Result<TransactionMoneyByCategoryModel[]>> Handle(
        GetIncomeExpenseReportByCategoryQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            return TimePeriodErrors.StartDateIsGreaterThanEndDate();
        }
        
        TransactionMoneyByCategoryModel[] incomesData = await transactionQueryRepository.GetTransactionsMoneyByCategoryAsync(
            request.WorkspaceId, TransactionType.Income, request.StartDate, request.EndDate, cancellationToken);
        TransactionMoneyByCategoryModel[] expensesData = await transactionQueryRepository.GetTransactionsMoneyByCategoryAsync(
            request.WorkspaceId, TransactionType.Expense, request.StartDate, request.EndDate, cancellationToken);
        
        return expensesData
            .UnionBy(incomesData, x => x.CategoryId)
            .ToArray();
    }
}
