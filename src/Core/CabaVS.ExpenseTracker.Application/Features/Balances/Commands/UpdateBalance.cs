using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record UpdateBalanceCommand(Guid WorkspaceId, Guid BalanceId, string Name, decimal Amount)
    : IRequest<Result>, IWorkspaceBoundedRequest;

internal sealed class UpdateBalanceCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBalanceCommand, Result>
{
    public async Task<Result> Handle(UpdateBalanceCommand request, CancellationToken cancellationToken)
    {
        var balanceRepository = unitOfWork.BalanceRepository;

        var balance = await balanceRepository.GetById(request.BalanceId, request.WorkspaceId, cancellationToken);
        if (balance is null) return BalanceErrors.NotFoundById(request.BalanceId);

        var balanceNameResult = BalanceName.Create(request.Name);
        if (balanceNameResult.IsFailure) return balanceNameResult;

        balance.Name = balanceNameResult.Value;
        balance.Amount = request.Amount;

        await balanceRepository.Update(balance, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return Result.Success();
    }
}