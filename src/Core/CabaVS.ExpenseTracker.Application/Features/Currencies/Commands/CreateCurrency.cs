using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Commands;

public sealed record CreateCurrencyCommand(string Name, string Code, string Symbol) : IRequest<Result<Guid>>, IAdminOnlyRequest;

internal sealed class CreateCurrencyCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCurrencyCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currencyResult = Currency.Create(
            Guid.NewGuid(),
            request.Name,
            request.Code,
            request.Symbol);
        if (currencyResult.IsFailure) return currencyResult.Error;

        var id = await unitOfWork.BuildCurrencyRepository().Create(currencyResult.Value, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return id;
    }
}