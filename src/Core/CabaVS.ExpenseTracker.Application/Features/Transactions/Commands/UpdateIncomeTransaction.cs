using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record UpdateIncomeTransactionCommand(
    Guid WorkspaceId, Guid IncomeTransactionId, DateOnly Date,
    Guid IncomeCategoryId, decimal AmountInIncomeCategoryCurrency,
    Guid BalanceId, decimal AmountInBalanceCurrency,
    IEnumerable<string> Tags)
    : IRequest<Result>, IWorkspaceBoundedRequest;

internal sealed class UpdateIncomeTransactionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateIncomeTransactionCommand, Result>
{
    public async Task<Result> Handle(UpdateIncomeTransactionCommand request, CancellationToken cancellationToken)
    {
        var incomeTransactionRepository = unitOfWork.BuildIncomeTransactionRepository();

        var incomeTransaction = await incomeTransactionRepository.GetById(
            request.IncomeTransactionId, request.WorkspaceId, cancellationToken);
        if (incomeTransaction is null) return TransactionErrors.NotFoundById(request.IncomeTransactionId);

        incomeTransaction.Date = request.Date;
        
        var tagsResult = TransactionTag.CreateMultiple(request.Tags);
        if (tagsResult.IsFailure) return tagsResult.Error;

        incomeTransaction.Tags = tagsResult.Value;

        if (incomeTransaction.Source.Id != request.IncomeCategoryId)
        {
            var incomeCategory = await unitOfWork.BuildIncomeCategoryRepository().GetById(
                request.IncomeCategoryId, request.WorkspaceId, cancellationToken);
            if (incomeCategory is null) return CategoryErrors.NotFoundById(request.IncomeCategoryId);

            incomeTransaction.Source = incomeCategory;
        }

        if (incomeTransaction.Destination.Id != request.BalanceId)
        {
            var balance = await unitOfWork.BuildBalanceRepository().GetById(
                request.BalanceId, request.WorkspaceId, cancellationToken);
            if (balance is null) return BalanceErrors.NotFoundById(request.BalanceId);
            
            incomeTransaction.ChangeDestination(balance);
        }

        incomeTransaction.AmountInSourceCurrency = request.AmountInIncomeCategoryCurrency;
        incomeTransaction.ChangeDestinationAmount(request.AmountInBalanceCurrency);

        await incomeTransactionRepository.Update(incomeTransaction, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success();
    }
}