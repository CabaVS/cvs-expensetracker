using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Services;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record DeleteTransactionCommand(Guid TransactionId, Guid WorkspaceId) 
    : IAuthenticatedUserRequest, IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class DeleteTransactionCommandHandler(
    IUnitOfWork unitOfWork,
    ITransactionService transactionService) : IRequestHandler<DeleteTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await unitOfWork.TransactionRepository.GetByIdAsync(request.TransactionId, cancellationToken);
        if (transaction is null)
        {
            return TransactionErrors.NotFoundById(request.TransactionId);
        }
        
        transactionService.RollbackTransaction(transaction);
        
        await unitOfWork.TransactionRepository.DeleteAsync(transaction, cancellationToken);
        switch (transaction.Type)
        {
            case TransactionType.Income:
                await unitOfWork.CategoryRepository.UpdateAsync((Category)transaction.Source, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Destination, cancellationToken);
                break;
            case TransactionType.Expense:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Source, cancellationToken);
                await unitOfWork.CategoryRepository.UpdateAsync((Category)transaction.Destination, cancellationToken);
                break;
            case TransactionType.Transfer:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Source, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Destination, cancellationToken);
                break;
            default:
                throw new InvalidOperationException();
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
