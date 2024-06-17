using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record CreateIncomeTransactionCommand(
    Guid WorkspaceId, DateOnly Date,
    Guid IncomeCategoryId, decimal AmountInIncomeCategoryCurrency,
    Guid BalanceId, decimal AmountInBalanceCurrency) : IRequest<Result<Guid>>, IWorkspaceBoundedRequest;

internal sealed class CreateIncomeTransactionCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateIncomeTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateIncomeTransactionCommand request, CancellationToken cancellationToken)
    {
        var balance = await unitOfWork.BalanceWriteRepository.GetById(
            request.BalanceId, request.WorkspaceId, cancellationToken);
        if (balance is null) return BalanceErrors.NotFoundById(request.BalanceId);

        var incomeCategory = await unitOfWork.IncomeCategoryWriteRepository.GetById(
            request.IncomeCategoryId, request.WorkspaceId, cancellationToken);
        if (incomeCategory is null) return CategoryErrors.NotFoundById(request.IncomeCategoryId);

        var incomeTransactionResult = IncomeTransaction.Create(
            Guid.NewGuid(),
            request.Date,
            incomeCategory,
            balance,
            request.AmountInIncomeCategoryCurrency,
            request.AmountInBalanceCurrency);
        if (incomeTransactionResult.IsFailure) return incomeTransactionResult.Error;

        var added = await unitOfWork.IncomeTransactionWriteRepository.Create(
            incomeTransactionResult.Value, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return added;
    }
}