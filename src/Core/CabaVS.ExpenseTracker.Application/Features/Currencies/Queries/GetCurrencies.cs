using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;

public sealed record GetCurrenciesQuery : IRequest<Result<CurrencyModel[]>>;

internal sealed class GetCurrenciesQueryHandler(
    ICurrencyReadRepository currencyReadRepository) : IRequestHandler<GetCurrenciesQuery, Result<CurrencyModel[]>>
{
    public async Task<Result<CurrencyModel[]>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        var allCurrencies = await currencyReadRepository.GetAll(cancellationToken);
        return allCurrencies;
    }
}