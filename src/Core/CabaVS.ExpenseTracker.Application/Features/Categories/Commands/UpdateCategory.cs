using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using CabaVS.ExpenseTracker.Domain.ValueObjects;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record UpdateCategoryCommand(Guid WorkspaceId, Guid CategoryId, string Name, decimal Amount) : IWorkspaceBoundRequest, IRequest<Result>;

internal sealed class UpdateCategoryCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<UpdateCategoryCommand, Result>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category? category = await unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category is null || category.Workspace.Id != request.WorkspaceId)
        {
            return CategoryErrors.NotFoundById(request.CategoryId);
        }

        Result<CategoryName> nameCreationResult = CategoryName.Create(request.Name);
        if (nameCreationResult.IsFailure)
        {
            return nameCreationResult.Error;
        }
        
        category.Name = nameCreationResult.Value;
        
        await unitOfWork.CategoryRepository.UpdateAsync(category, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
