using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Common.Abstractions;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Categories.Commands;

public sealed record DeleteExpenseCategoryCommand(
    Guid Id, Guid WorkspaceId) : IRequest<Result>, IWorkspaceBoundedRequest;
    
internal sealed class DeleteExpenseCategoryCommandHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteExpenseCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteExpenseCategoryCommand request, CancellationToken cancellationToken)
    {
        var repository = unitOfWork.ExpenseCategoryRepository;

        var expenseCategory = await repository.GetById(request.Id, request.WorkspaceId, cancellationToken);
        if (expenseCategory is null) return CategoryErrors.NotFoundById(request.Id);

        await repository.Delete(expenseCategory, request.WorkspaceId, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        return Result.Success();
    }
}