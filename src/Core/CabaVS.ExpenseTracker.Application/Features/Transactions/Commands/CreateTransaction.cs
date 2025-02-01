using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Services;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Entities.Abstractions;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record CreateTransactionCommand(Guid WorkspaceId,
    DateOnly Date, string[] Tags, decimal AmountInSourceCurrency, decimal AmountInDestinationCurrency,
    TransactionType Type, Guid SourceId, Guid DestinationId) 
    : IAuthenticatedUserRequest, IWorkspaceBoundRequest, IRequest<Result<Guid>>;

internal sealed class CreateTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    ITransactionService transactionService) : IRequestHandler<CreateTransactionCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        Workspace? workspace = await unitOfWork.WorkspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace is null)
        {
            return WorkspaceErrors.NotFoundById(request.WorkspaceId);
        }

        IWithCurrency? source;
        IWithCurrency? destination;

        switch (request.Type)
        {
            case TransactionType.Income:
                source = await unitOfWork.CategoryRepository.GetByIdAsync(request.SourceId, cancellationToken);
                if (source is null)
                {
                    return CategoryErrors.NotFoundById(request.SourceId);
                }
                
                destination = await unitOfWork.BalanceRepository.GetByIdAsync(request.DestinationId, cancellationToken);
                if (destination is null)
                {
                    return BalanceErrors.NotFoundById(request.DestinationId);
                }
                
                break;
            case TransactionType.Expense:
                source = await unitOfWork.BalanceRepository.GetByIdAsync(request.SourceId, cancellationToken);
                if (source is null)
                {
                    return BalanceErrors.NotFoundById(request.SourceId);
                }
                
                destination = await unitOfWork.CategoryRepository.GetByIdAsync(request.DestinationId, cancellationToken);
                if (destination is null)
                {
                    return CategoryErrors.NotFoundById(request.DestinationId);
                }
                
                break;
            case TransactionType.Transfer:
                source = await unitOfWork.BalanceRepository.GetByIdAsync(request.SourceId, cancellationToken);
                if (source is null)
                {
                    return BalanceErrors.NotFoundById(request.SourceId);
                }
                
                destination = await unitOfWork.BalanceRepository.GetByIdAsync(request.DestinationId, cancellationToken);
                if (destination is null)
                {
                    return BalanceErrors.NotFoundById(request.DestinationId);
                }
                
                break;
            default:
                throw new InvalidOperationException();
        }
        
        Result<Transaction> transactionCreationResult = Transaction.Create(
            request.Date, request.Tags, request.AmountInSourceCurrency, request.AmountInDestinationCurrency,
            request.Type, source, destination, workspace);
        if (transactionCreationResult.IsFailure)
        {
            return transactionCreationResult.Error;
        }
        
        transactionService.ApplyTransaction(transactionCreationResult.Value);
        
        Guid createdId = await unitOfWork.TransactionRepository.CreateAsync(transactionCreationResult.Value, cancellationToken);
        switch (request.Type)
        {
            case TransactionType.Income:
                await unitOfWork.CategoryRepository.UpdateAsync((Category)source, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)destination, cancellationToken);
                break;
            case TransactionType.Expense:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)source, cancellationToken);
                await unitOfWork.CategoryRepository.UpdateAsync((Category)destination, cancellationToken);
                break;
            case TransactionType.Transfer:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)source, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)destination, cancellationToken);
                break;
            default:
                throw new InvalidOperationException();
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return createdId;
    }
}
