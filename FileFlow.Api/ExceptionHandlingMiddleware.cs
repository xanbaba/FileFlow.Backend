using FileFlow.Application.Services.Exceptions;
using FluentValidation;
using JetBrains.Annotations;
using FileNotFoundException = FileFlow.Application.Services.Exceptions.FileNotFoundException;

namespace FileFlow.Api;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (BadHttpRequestException e)
        {
            context.Response.StatusCode = e.StatusCode;
            await context.Response.WriteAsJsonAsync(new ErrorMessage(e.Message));
        }

        catch (ValidationException e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationFailureResponse = new ValidationFailureResponse
            {
                Errors = e.Errors.Select(x => new ValidationResponse
                {
                    PropertyName = x.PropertyName,
                    Message = x.ErrorMessage
                })
            };

            await context.Response.WriteAsJsonAsync(validationFailureResponse);
        }
        catch (Exception e) when (e is FileNotFoundException or FolderNotFoundException or ItemNotFoundException
                                      or UserNotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ErrorMessage(e.Message));
        }
        catch (Exception e) when (e is FileAlreadyExistsException or FolderAlreadyExistsException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new ErrorMessage(e.Message));
        }
        catch (Exception e) when (e is InvalidOperationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            logger.LogError(e, "Invalid operation exception occurred.");
            await context.Response.WriteAsJsonAsync(new ErrorMessage(e.Message));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorMessage("An unexpected error occurred."));
        }
    }
}

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationResponse> Errors { [UsedImplicitly] get; init; }
}

public class ValidationResponse
{
    public required string PropertyName { [UsedImplicitly] get; init; }

    public required string Message { [UsedImplicitly] get; init; }
}

public record ErrorMessage([UsedImplicitly] string Message);