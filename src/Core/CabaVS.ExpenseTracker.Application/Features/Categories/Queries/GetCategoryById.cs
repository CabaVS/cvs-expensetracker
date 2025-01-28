using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid CategoryId, Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<CategoryModel>>;

internal sealed class GetCategoryByIdQueryHandler(
    ICategoryQueryRepository categoryQueryRepository) : IRequestHandler<GetCategoryByIdQuery, Result<CategoryModel>>
{
    public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        CategoryModel? categoryModel = await categoryQueryRepository.GetByIdAsync(request.WorkspaceId, request.CategoryId, cancellationToken);
        return categoryModel is not null ? categoryModel : CategoryErrors.NotFoundById(request.CategoryId);
    }
}
