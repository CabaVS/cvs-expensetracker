using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Application.Features.Categories.Models;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Queries;

public sealed record GetIncomeCategoryByIdQuery(
    Guid WorkspaceId,
    Guid CategoryId) : IRequest<Result<IncomeCategoryModel>>, IWorkspaceBoundedRequest;

internal sealed class GetIncomeCategoryByIdQueryHandler(
    IIncomeCategoryReadRepository incomeCategoryReadRepository) : IRequestHandler<GetIncomeCategoryByIdQuery, Result<IncomeCategoryModel>>
{
    public async Task<Result<IncomeCategoryModel>> Handle(GetIncomeCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var model = await incomeCategoryReadRepository.GetById(
            request.CategoryId, request.WorkspaceId, cancellationToken);
        if (model is null) return CategoryErrors.NotFoundById(request.CategoryId);

        return model;
    }
}