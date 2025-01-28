using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetAllCategoriesQuery(Guid WorkspaceId) : IWorkspaceBoundRequest, IRequest<Result<CategoryModel[]>>;

internal sealed class GetAllCategoriesHandler(ICategoryQueryRepository categoryQueryRepository) 
    : IRequestHandler<GetAllCategoriesQuery, Result<CategoryModel[]>>
{
    public async Task<Result<CategoryModel[]>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        CategoryModel[] allModels = await categoryQueryRepository.GetAllAsync(request.WorkspaceId, cancellationToken);
        return allModels;
    }
}
