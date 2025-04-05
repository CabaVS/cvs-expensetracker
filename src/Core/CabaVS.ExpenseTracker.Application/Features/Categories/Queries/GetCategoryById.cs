using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetCategoryByIdQuery(Guid WorkspaceId, Guid CategoryId)
    : IWorkspaceBoundedRequest, IRequest<Result<CategoryModel>>;

internal sealed class GetCategoryByIdQueryHandler(IReadOnlyCategoryRepository readOnlyCategoryRepository)
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryModel>>
{
    public async Task<Result<CategoryModel>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        CategoryModel? categoryModel = await readOnlyCategoryRepository.GetByIdAsync(
            request.WorkspaceId,
            request.CategoryId,
            cancellationToken);
        return categoryModel is not null
            ? categoryModel
            : CategoryErrors.NotFoundById(request.CategoryId);
    }
}
