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
        Currency? currency = await unitOfWork.CurrencyRepository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.NotFoundById(request.CurrencyId);
        }
        
        Workspace? workspace = await unitOfWork.WorkspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace is null)
        {
            return WorkspaceErrors.NotFoundById(request.WorkspaceId);
        }

        Result<Balance> balanceCreationResult = Balance.Create(
            request.Name,
            request.Amount,
            currency,
            workspace);
        if (balanceCreationResult.IsFailure)
        {
            return balanceCreationResult.Error;
        }
        
        Guid balanceId = await unitOfWork.BalanceRepository.CreateAsync(balanceCreationResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return balanceId;
    }
}
