using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;

public sealed record GetAllCurrenciesQuery : IRequest<Result<CurrencyModel[]>>;

internal sealed class GetAllCurrenciesQueryHandler(IReadOnlyCurrencyRepository readOnlyCurrencyRepository)
    : IRequestHandler<GetAllCurrenciesQuery, Result<CurrencyModel[]>>
{
    public async Task<Result<CurrencyModel[]>> Handle(GetAllCurrenciesQuery request, CancellationToken cancellationToken)
    {
        CurrencyModel[] models = await readOnlyCurrencyRepository.GetAllAsync(cancellationToken);
        return Result<CurrencyModel[]>.Success(models);
    }
}
