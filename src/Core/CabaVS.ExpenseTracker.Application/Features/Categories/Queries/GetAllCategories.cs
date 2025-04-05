using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(Guid WorkspaceId) 
    : IWorkspaceBoundedRequest, IRequest<Result<CategoryModel[]>>;

internal sealed class GetAllCategoriesQueryHandler(IReadOnlyCategoryRepository readOnlyCategoryRepository) 
    : IRequestHandler<GetAllCategoriesQuery, Result<CategoryModel[]>>
{
    public async Task<Result<CategoryModel[]>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        CategoryModel[] models = await readOnlyCategoryRepository.GetAllAsync(request.WorkspaceId, cancellationToken);
        return models;
    }
}
