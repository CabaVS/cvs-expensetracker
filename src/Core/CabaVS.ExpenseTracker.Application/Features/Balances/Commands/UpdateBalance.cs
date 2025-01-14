using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
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
        IBalanceRepository balanceRepository = unitOfWork.BuildBalanceRepository();
        
        Balance? balance = await balanceRepository.GetById(request.WorkspaceId, request.BalanceId, cancellationToken);
        if (balance is null)
        {
            return BalanceErrors.NotFoundById(request.BalanceId);
        }

        Result<BalanceName> balanceNameResult = BalanceName.Create(request.Name);
        if (balanceNameResult.IsFailure)
        {
            return balanceNameResult.Error;
        }
        
        balance.Name = balanceNameResult.Value;
        balance.Amount = request.Amount;
        
        await balanceRepository.Update(request.WorkspaceId, balance, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}
