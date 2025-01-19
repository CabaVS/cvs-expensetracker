using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.TransferTransactions.Commands;

public sealed record CreateTransferTransactionCommand(
    Guid WorkspaceId,
    Guid SourceId,
    Guid DestinationId,
    Guid CurrencyId,
    decimal Amount,
    decimal? AmountInSourceCurrency,
    decimal? AmountInDestinationCurrency,
    DateOnly Date,
    string[] Tags) : IWorkspaceBoundRequest, IRequest<Result<Guid>>;
    
internal sealed class CreateTransferTransactionCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateTransferTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTransferTransactionCommand request, CancellationToken cancellationToken)
    {
        Currency? currency = await unitOfWork.CurrencyRepository.GetById(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.NotFoundById(request.CurrencyId);
        }

        Balance? source = await unitOfWork.BalanceRepository.GetById(request.WorkspaceId, request.SourceId, cancellationToken);
        if (source is null)
        {
            return BalanceErrors.NotFoundById(request.SourceId);
        }

        Balance? destination = await unitOfWork.BalanceRepository.GetById(request.WorkspaceId, request.DestinationId, cancellationToken);
        if (destination is null)
        {
            return BalanceErrors.NotFoundById(request.DestinationId);
        }

        Result<TransferTransaction> creationResult = TransferTransaction.Create(
            Guid.NewGuid(),
            request.Date,
            request.Tags,
            request.Amount,
            currency,
            request.AmountInSourceCurrency ?? request.Amount,
            source,
            request.AmountInDestinationCurrency ?? request.Amount,
            destination,
            true);
        if (creationResult.IsFailure)
        {
            return creationResult.Error;
        }

        Guid createdId = await unitOfWork.TransferTransactionRepository.Create(creationResult.Value, cancellationToken);
        await unitOfWork.BalanceRepository.Update(request.WorkspaceId, source, cancellationToken);
        await unitOfWork.BalanceRepository.Update(request.WorkspaceId, destination, cancellationToken);
        
        await unitOfWork.SaveChanges(cancellationToken);
        
        return createdId;
    }
}
