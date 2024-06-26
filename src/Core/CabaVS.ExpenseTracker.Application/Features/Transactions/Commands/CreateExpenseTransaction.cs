using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record CreateExpenseTransactionCommand(
    Guid WorkspaceId, DateOnly Date,
    Guid ExpenseCategoryId, decimal AmountInExpenseCategoryCurrency,
    Guid BalanceId, decimal AmountInBalanceCurrency,
    IEnumerable<string> Tags) : IRequest<Result<Guid>>, IWorkspaceBoundedRequest;

internal sealed class CreateExpenseTransactionCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExpenseTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExpenseTransactionCommand request, CancellationToken cancellationToken)
    {
        var balance = await unitOfWork.BuildBalanceRepository().GetById(
            request.BalanceId, request.WorkspaceId, cancellationToken);
        if (balance is null) return BalanceErrors.NotFoundById(request.BalanceId);

        var expenseCategory = await unitOfWork.BuildExpenseCategoryRepository().GetById(
            request.ExpenseCategoryId, request.WorkspaceId, cancellationToken);
        if (expenseCategory is null) return CategoryErrors.NotFoundById(request.ExpenseCategoryId);

        var expenseTransactionResult = ExpenseTransaction.Create(
            Guid.NewGuid(),
            request.Date,
            balance,
            expenseCategory,
            request.AmountInBalanceCurrency,
            request.AmountInExpenseCategoryCurrency,
            request.Tags,
            true);
        if (expenseTransactionResult.IsFailure) return expenseTransactionResult.Error;

        var added = await unitOfWork.BuildExpenseTransactionRepository().Create(
            expenseTransactionResult.Value, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return added;
    }
}