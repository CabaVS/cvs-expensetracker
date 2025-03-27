using CabaVS.ExpenseTracker.Application.Common;
using CabaVS.ExpenseTracker.Application.Contracts.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Features.Users.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using CabaVS.ExpenseTracker.Domain.Errors.Shared;
using CabaVS.ExpenseTracker.Domain.Shared;
using MediatR;

namespace CabaVS.ExpenseTracker.Application.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid UserId) : IUserOwningRequest, IRequest<Result<UserModel>>;

internal sealed class GetUserByIdQueryHandler(
    IReadOnlyUserRepository readOnlyUserRepository) : IRequestHandler<GetUserByIdQuery, Result<UserModel>>
{
    public async Task<Result<UserModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        UserModel? userModel = await readOnlyUserRepository.GetByIdAsync(request.UserId, cancellationToken);
        return userModel is not null
            ? Result<UserModel>.Success(
                userModel)
            : Result<UserModel>.Failure(
                CommonErrors.NotFoundById(nameof(User), request.UserId));
    }
}
