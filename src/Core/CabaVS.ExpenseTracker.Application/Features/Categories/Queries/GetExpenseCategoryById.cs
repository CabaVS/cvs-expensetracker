using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetExpenseCategoryByIdQuery(Guid WorkspaceId, Guid ExpenseCategoryId)
    : IRequest<Result<ExpenseCategoryModel>>, IWorkspaceBoundedRequest;

internal sealed class GetExpenseCategoryByIdQueryHandler(IExpenseCategoryReadRepository expenseCategoryReadRepository)
    : IRequestHandler<GetExpenseCategoryByIdQuery, Result<ExpenseCategoryModel>>
{
    public async Task<Result<ExpenseCategoryModel>> Handle(GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await expenseCategoryReadRepository.GetById(
            request.ExpenseCategoryId, request.WorkspaceId, cancellationToken);
        if (model is null) return CategoryErrors.NotFoundById(request.ExpenseCategoryId);

        return model;
    }
}