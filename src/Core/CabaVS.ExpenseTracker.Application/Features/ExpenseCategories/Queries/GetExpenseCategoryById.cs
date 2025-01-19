using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.ExpenseCategories.Queries;

public sealed record GetExpenseCategoryByIdQuery(Guid WorkspaceId, Guid ExpenseCategoryId)
    : IWorkspaceBoundRequest, IRequest<Result<ExpenseCategoryModel>>;

internal sealed class GetExpenseCategoryByIdQueryHandler(IExpenseCategoryReadRepository expenseCategoryReadRepository)
    : IRequestHandler<GetExpenseCategoryByIdQuery, Result<ExpenseCategoryModel>>
{
    public async Task<Result<ExpenseCategoryModel>> Handle(
        GetExpenseCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        ExpenseCategoryModel? expenseCategoryModel = await expenseCategoryReadRepository.GetById(
            request.WorkspaceId, request.ExpenseCategoryId, cancellationToken); 
        return expenseCategoryModel is not null
            ? expenseCategoryModel
            : CategoryErrors.ExpenseCategoryNotFoundById(request.ExpenseCategoryId);
    }
}
