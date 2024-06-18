using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record CreateExpenseCategoryCommand(
    string Name, Guid CurrencyId, Guid WorkspaceId) : IRequest<Result<Guid>>, IWorkspaceBoundedRequest;

internal sealed class CreateExpenseCategoryCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<CreateExpenseCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var currency = await unitOfWork.CurrencyWriteRepository.GetById(request.CurrencyId, cancellationToken);
        if (currency is null) return CurrencyErrors.NotFoundById(request.CurrencyId);

        var expenseCategoryResult = ExpenseCategory.Create(Guid.NewGuid(), request.Name, currency);
        if (expenseCategoryResult.IsFailure) return expenseCategoryResult.Error;

        var added = await unitOfWork.ExpenseCategoryWriteRepository.Create(
            expenseCategoryResult.Value,
            request.WorkspaceId,
            cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);
        
        return added;
    }
}