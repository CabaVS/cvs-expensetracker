using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record UpdateBalanceCommand(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount) : IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class UpdateBalanceCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateBalanceCommand, Result>
{
    public async Task<Result> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        Balance? balance = await unitOfWork.BalanceRepository.GetById(request.WorkspaceId, request.BalanceId, cancellationToken);
        if (balance is null)
        {
            return BalanceErrors.NotFoundById(request.BalanceId);
        }
        
        Result updateNameResult = balance.UpdateName(request.Name);
        if (updateNameResult.IsFailure)
        {
            return updateNameResult.Error;
        }
        
        Result updateAmountResult = balance.UpdateAmount(request.Amount);
        if (updateAmountResult.IsFailure)
        {
            return updateAmountResult.Error;
        }
        
        await unitOfWork.BalanceRepository.Update(request.WorkspaceId, balance, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}
