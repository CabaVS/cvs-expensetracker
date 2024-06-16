using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetAllIncomeCategoriesQuery(Guid WorkspaceId) : IRequest<Result<IncomeCategoryModel[]>>, IWorkspaceBoundedRequest;

internal sealed class GetAllIncomeCategoriesQueryHandler(
    IIncomeCategoryReadRepository incomeCategoryReadRepository) : IRequestHandler<GetAllIncomeCategoriesQuery, Result<IncomeCategoryModel[]>>
{
    public async Task<Result<IncomeCategoryModel[]>> Handle(GetAllIncomeCategoriesQuery request, CancellationToken cancellationToken)
    {
        var models = await incomeCategoryReadRepository.GetAll(request.WorkspaceId, cancellationToken);
        return models;
    }
}