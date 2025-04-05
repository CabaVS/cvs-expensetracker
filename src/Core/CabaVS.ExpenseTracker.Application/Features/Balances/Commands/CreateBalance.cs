using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record CreateBalanceCommand(Guid WorkspaceId, string Name, decimal Amount, Guid CurrencyId) 
    : IWorkspaceBoundedRequest, IRequest<Result<Guid>>;

internal sealed class CreateBalanceCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateBalanceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBalanceCommand request, CancellationToken cancellationToken)
    {
        Currency? currency = await unitOfWork.CurrencyRepository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.CurrencyNotFoundById(request.CurrencyId);
        }
        
        Result<Balance> balanceCreationResult = Balance.CreateNew(request.Name, request.Amount, currency);
        if (balanceCreationResult.IsFailure)
        {
            return balanceCreationResult.Error;
        }
        
        Guid idOfCreated = await unitOfWork.BalanceRepository.CreateAsync(
            balanceCreationResult.Value,
            request.WorkspaceId,
            cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return idOfCreated;
    }
}
