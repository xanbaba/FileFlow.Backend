using System.Security.Claims;
using FileFlow.Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FileFlow.Application;

public class UserStorageMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        using var scope = scopeFactory.CreateScope();
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userStorageService = scope.ServiceProvider.GetRequiredService<IUserStorageService>();
        if (userId is not null)
        {
            await userStorageService.CreateIfNotExistsAsync(userId);
        }
        await next(context);
    }
}