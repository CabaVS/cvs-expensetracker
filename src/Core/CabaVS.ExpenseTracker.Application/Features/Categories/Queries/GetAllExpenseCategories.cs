using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetAllExpenseCategoriesQuery(Guid WorkspaceId) : IRequest<Result<ExpenseCategoryModel[]>>, IWorkspaceBoundedRequest;

internal sealed class GetAllExpenseCategoriesQueryHandler(
    IExpenseCategoryReadRepository expenseCategoryReadRepository) : IRequestHandler<GetAllExpenseCategoriesQuery, Result<ExpenseCategoryModel[]>>
{
    public async Task<Result<ExpenseCategoryModel[]>> Handle(GetAllExpenseCategoriesQuery request, CancellationToken cancellationToken)
    {
        var models = await expenseCategoryReadRepository.GetAll(request.WorkspaceId, cancellationToken);
        return models;
    }
}