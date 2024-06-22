using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record CreateIncomeCategoryCommand(
    string Name, Guid CurrencyId, Guid WorkspaceId) : IRequest<Result<Guid>>, IWorkspaceBoundedRequest;

internal sealed class CreateIncomeCategoryCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateIncomeCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateIncomeCategoryCommand request, CancellationToken cancellationToken)
    {
        var currency = await unitOfWork.BuildCurrencyRepository().GetById(request.CurrencyId, cancellationToken);
        if (currency is null) return CurrencyErrors.NotFoundById(request.CurrencyId);

        var incomeCategoryResult = IncomeCategory.Create(
            Guid.NewGuid(),
            request.Name,
            currency);
        if (incomeCategoryResult.IsFailure) return incomeCategoryResult.Error;

        var added = await unitOfWork.BuildIncomeCategoryRepository().Create(
            incomeCategoryResult.Value, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return added;
    }
}