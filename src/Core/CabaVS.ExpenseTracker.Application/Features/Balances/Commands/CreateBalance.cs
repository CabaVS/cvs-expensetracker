using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record CreateBalanceCommand(Guid WorkspaceId, Guid CurrencyId, string Name, decimal Amount) 
    : IWorkspaceBoundRequest, IRequest<Result<Guid>>;

internal sealed class CreateBalanceCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateBalanceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBalanceCommand request, CancellationToken cancellationToken)
    {
        var currency = await unitOfWork.BuildCurrencyRepository().GetById(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.NotFoundById(request.CurrencyId);
        }

        var balanceCreationResult = Balance.Create(
            Guid.NewGuid(),
            request.Name,
            request.Amount,
            currency);
        if (balanceCreationResult.IsFailure)
        {
            return balanceCreationResult.Error;
        }
        
        var balanceId = await unitOfWork.BuildBalanceRepository()
            .Create(request.WorkspaceId, balanceCreationResult.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return balanceId;
    }
}