using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Currencies.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Currencies.Queries;

public sealed record GetCurrencyByIdQuery(Guid Id) : IRequest<Result<CurrencyModel>>;

public sealed class GetCurrencyByIdQueryHandler(
    ICurrencyReadRepository currencyReadRepository) : IRequestHandler<GetCurrencyByIdQuery, Result<CurrencyModel>>
{
    public async Task<Result<CurrencyModel>> Handle(GetCurrencyByIdQuery request, CancellationToken cancellationToken)
    {
        var currencyModel = await currencyReadRepository.GetById(request.Id, cancellationToken);

        return currencyModel is not null
            ? currencyModel
            : CurrencyErrors.NotFoundById(request.Id);
    }
}