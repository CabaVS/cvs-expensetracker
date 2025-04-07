using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Domain.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(Guid WorkspaceId,
    DateOnly Date, TransactionType Type, IEnumerable<string> Tags,
    decimal AmountInSourceCurrency, decimal AmountInDestinationCurrency,
    Guid SourceId, Guid DestinationId)
    : IWorkspaceBoundedRequest, IRequest<Result<Guid>>;

internal sealed class CreateTransactionCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        IWithCurrency? source = request.Type switch
        {
            TransactionType.Expense or TransactionType.Transfer => await unitOfWork.BalanceRepository.GetByIdAsync(
                request.WorkspaceId, request.SourceId, cancellationToken),
            TransactionType.Income => await unitOfWork.CategoryRepository.GetByIdAsync(
                request.WorkspaceId, request.SourceId, cancellationToken),
            _ => throw new InvalidOperationException()
        };
        if (source is null)
        {
            return TransactionErrors.SourceOrDestinationNotFound();
        }
        
        IWithCurrency? destination = request.Type switch
        {
            TransactionType.Income or TransactionType.Transfer => await unitOfWork.BalanceRepository.GetByIdAsync(
                request.WorkspaceId, request.DestinationId, cancellationToken),
            TransactionType.Expense => await unitOfWork.CategoryRepository.GetByIdAsync(
                request.WorkspaceId, request.DestinationId, cancellationToken),
            _ => throw new InvalidOperationException()
        };
        if (destination is null)
        {
            return TransactionErrors.SourceOrDestinationNotFound();
        }
        
        Result<Transaction> creationResult = Transaction.CreateNew(
            request.Date, request.Type, request.Tags,
            request.AmountInSourceCurrency, request.AmountInDestinationCurrency,
            source, destination);
        if (creationResult.IsFailure)
        {
            return creationResult.Error;
        }
        
        Guid createdId = await unitOfWork.TransactionRepository.AddAsync(creationResult.Value, request.WorkspaceId, cancellationToken);
        switch (request.Type)
        {
            case TransactionType.Expense:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)source, request.WorkspaceId, cancellationToken);
                await unitOfWork.CategoryRepository.UpdateAsync((Category)destination, request.WorkspaceId, cancellationToken);
                break;
            case TransactionType.Income:
                await unitOfWork.CategoryRepository.UpdateAsync((Category)source, request.WorkspaceId, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)destination, request.WorkspaceId, cancellationToken);
                break;
            case TransactionType.Transfer:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)source, request.WorkspaceId, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)destination, request.WorkspaceId, cancellationToken);
                break;
            default:
                throw new InvalidOperationException();
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return createdId;
    }
}
