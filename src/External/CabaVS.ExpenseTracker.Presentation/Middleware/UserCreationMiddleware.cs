using CabaVS.ExpenseTracker.Application.Abstractions.Persistence;
using CabaVS.ExpenseTracker.Application.Abstractions.Persistence.Repositories;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation;
using CabaVS.ExpenseTracker.Application.Abstractions.Presentation.Models;
using CabaVS.ExpenseTracker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CabaVS.ExpenseTracker.Presentation.Middleware;

internal sealed class UserCreationMiddleware(
    IUnitOfWork unitOfWork,
    IUserReadRepository userReadRepository,
    ICurrentUserAccessor currentUserAccessor,
    ILogger<UserCreationMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity?.IsAuthenticated != true)
        {
            await next(context);
            return;
        }
        
        AuthenticatedUserModel? currentUser = await currentUserAccessor.GetCurrentUser();
        if (currentUser is null)
        {
            logger.LogWarning("User is authenticated, but returned Current User is null.");
            
            await next(context);
            return;
        }
        
        if (!await userReadRepository.IsExistById(currentUser.Id))
        {
            logger.LogInformation("Creating User with ID '{UserId}'...", currentUser.Id);

            var userToCreate = new User(currentUser.Id, DateTime.UtcNow, null);
            
            await unitOfWork.UserRepository.Create(userToCreate);
            await unitOfWork.SaveChanges();
        }
        
        await next(context);
    }
}
