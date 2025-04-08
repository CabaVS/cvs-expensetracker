using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Transactions.Commands;

public sealed record DeleteTransactionCommand(Guid WorkspaceId, Guid TransactionId) : IWorkspaceBoundedRequest, IRequest<Result>;

internal sealed class DeleteTransactionCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<DeleteTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        Transaction? transaction = await unitOfWork.TransactionRepository.GetByIdAsync(
            request.TransactionId,
            request.WorkspaceId,
            cancellationToken);
        if (transaction is null)
        {
            return TransactionErrors.NotFoundById(request.TransactionId);
        }
        
        transaction.Rollback();
        
        await unitOfWork.TransactionRepository.RemoveAsync(transaction, cancellationToken);
        switch (transaction.Type)
        {
            case TransactionType.Expense:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Source, request.WorkspaceId, cancellationToken);
                break;
            case TransactionType.Income:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Destination, request.WorkspaceId, cancellationToken);
                break;
            case TransactionType.Transfer:
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Source, request.WorkspaceId, cancellationToken);
                await unitOfWork.BalanceRepository.UpdateAsync((Balance)transaction.Destination, request.WorkspaceId, cancellationToken);
                break;
            default:
                throw new InvalidOperationException();
        }
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
