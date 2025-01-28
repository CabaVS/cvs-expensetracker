using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record UpdateBalanceCommand(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount) : IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class UpdateBalanceCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateBalanceCommand, Result>
{
    public async Task<Result> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        Balance? balance = await unitOfWork.BalanceRepository.GetByIdAsync(request.BalanceId, cancellationToken);
        if (balance is null || balance.Workspace.Id != request.WorkspaceId)
        {
            return BalanceErrors.NotFoundById(request.BalanceId);
        }

        Result<BalanceName> nameCreationResult = BalanceName.Create(request.Name);
        if (nameCreationResult.IsFailure)
        {
            return nameCreationResult.Error;
        }
        
        balance.Name = nameCreationResult.Value;
        balance.Amount = request.Amount;
        
        await unitOfWork.BalanceRepository.UpdateAsync(balance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
