using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Balances.Commands;

public sealed record CreateBalanceCommand(string Name, decimal Amount, Guid CurrencyId, Guid WorkspaceId)
    : IRequest<Result<Guid>>, IWorkspaceBoundedRequest;

internal sealed class CreateBalanceCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateBalanceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBalanceCommand request, CancellationToken cancellationToken)
    {
        var currency = await unitOfWork.CurrencyWriteRepository.GetById(request.CurrencyId, cancellationToken);
        if (currency is null) return CurrencyErrors.NotFoundById(request.CurrencyId);
        
        var balanceResult = Balance.Create(
            Guid.NewGuid(),
            request.Name,
            request.Amount,
            currency);
        if (balanceResult.IsFailure) return balanceResult.Error;

        var added = await unitOfWork.BalanceWriteRepository.Create(balanceResult.Value, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return added;
    }
}