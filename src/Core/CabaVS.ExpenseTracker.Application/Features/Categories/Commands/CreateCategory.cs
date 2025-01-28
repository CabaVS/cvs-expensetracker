using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(Guid WorkspaceId, Guid CurrencyId, string Name, CategoryType Type) 
    : IWorkspaceBoundRequest, IRequest<Result<Guid>>;

internal sealed class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Currency? currency = await unitOfWork.CurrencyRepository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.NotFoundById(request.CurrencyId);
        }
        
        Workspace? workspace = await unitOfWork.WorkspaceRepository.GetByIdAsync(request.WorkspaceId, cancellationToken);
        if (workspace is null)
        {
            return WorkspaceErrors.NotFoundById(request.WorkspaceId);
        }

        Result<Category> categoryCreationResult = Category.Create(
            request.Name,
            request.Type,
            currency,
            workspace);
        if (categoryCreationResult.IsFailure)
        {
            return categoryCreationResult.Error;
        }
        
        Guid categoryId = await unitOfWork.CategoryRepository.CreateAsync(categoryCreationResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return categoryId;
    }
}
