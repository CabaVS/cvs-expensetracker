using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record DeleteBalanceCommand(Guid WorkspaceId, Guid BalanceId) : IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class DeleteBalanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBalanceCommand, Result>
{
    public async Task<Result> Handle(DeleteBalanceCommand request, CancellationToken cancellationToken)
    {
        Balance? balance = await unitOfWork.BalanceRepository.GetByIdAsync(request.BalanceId, cancellationToken);
        if (balance is null || balance.Workspace.Id != request.WorkspaceId)
        {
            return BalanceErrors.NotFoundById(request.BalanceId);
        }
        
        await unitOfWork.BalanceRepository.DeleteAsync(balance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
