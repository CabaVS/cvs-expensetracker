using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record DeleteCategoryCommand(Guid WorkspaceId, Guid CategoryId) : IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null || category.Workspace.Id != request.WorkspaceId)
        {
            return CategoryErrors.NotFoundById(request.CategoryId);
        }
        
        await unitOfWork.CategoryRepository.DeleteAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
