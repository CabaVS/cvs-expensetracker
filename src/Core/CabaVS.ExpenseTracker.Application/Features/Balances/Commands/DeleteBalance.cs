using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
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
        var balanceRepository = unitOfWork.BuildBalanceRepository();
        
        var balance = await balanceRepository.GetById(request.WorkspaceId, request.BalanceId, cancellationToken);
        if (balance is null)
        {
            return BalanceErrors.NotFoundById(request.BalanceId);
        }
        
        await balanceRepository.Delete(request.WorkspaceId, balance, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}