using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;

public sealed record GetCurrenciesQuery : IAuthenticatedUserRequest, IRequest<Result<CurrencyModel[]>>;

internal sealed class GetCurrenciesQueryHandler(
    ICurrencyQueryRepository currencyQueryRepository) : IRequestHandler<GetCurrenciesQuery, Result<CurrencyModel[]>>
{
    public async Task<Result<CurrencyModel[]>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
    {
        CurrencyModel[] allCurrencies = await currencyQueryRepository.GetAllAsync(cancellationToken);
        return allCurrencies;
    }
}
