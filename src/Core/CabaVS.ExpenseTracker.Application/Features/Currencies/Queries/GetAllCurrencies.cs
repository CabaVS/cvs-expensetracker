using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;

public sealed record GetAllCurrenciesQuery : IRequest<Result<CurrencyModel[]>>;

internal sealed class GetAllCurrenciesQueryHandler(
    ICurrencyReadRepository currencyReadRepository) : IRequestHandler<GetAllCurrenciesQuery, Result<CurrencyModel[]>>
{
    public async Task<Result<CurrencyModel[]>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var models = await currencyReadRepository.GetAll(cancellationToken);
        return models;
    }
}