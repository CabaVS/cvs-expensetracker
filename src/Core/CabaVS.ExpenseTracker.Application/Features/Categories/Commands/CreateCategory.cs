using CabaVS.ExpenseTracker.Application.Common.Requests;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Enums;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record CreateCategoryCommand(Guid WorkspaceId, string Name, CategoryType Type, Guid CurrencyId)
    : IWorkspaceBoundedRequest, IRequest<Result<Guid>>;

internal sealed class CreateCategoryCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateCategoryCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Currency? currency = await unitOfWork.CurrencyRepository.GetByIdAsync(request.CurrencyId, cancellationToken);
        if (currency is null)
        {
            return CurrencyErrors.CurrencyNotFoundById(request.CurrencyId);
        }
        
        Result<Category> categoryCreationResult = Category.CreateNew(request.Name, request.Type, currency);
        if (categoryCreationResult.IsFailure)
        {
            return categoryCreationResult.Error;
        }
        
        Guid idOfCreated = await unitOfWork.CategoryRepository.CreateAsync(
            categoryCreationResult.Value,
            request.WorkspaceId,
            cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return idOfCreated;
    }
}
