using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Extensions;
using SoccerManager.Application.Core.Exceptions;

namespace SoccerManager.API.Middleware;

/// <summary>
/// Catches unhandled exceptions raised further down the pipeline and translates them into a problem-details response.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next delegate in the request pipeline.</param>
    /// <param name="logger">The logger used to record unhandled exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the next delegate in the pipeline and converts any exception it throws into a problem-details response.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that completes once the request has been handled.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            // A response already writing to the client cannot have its status code or body replaced.
            if (context.Response.HasStarted)
                throw;

            await context.WriteProblemAsync(BuildValidationProblem(ex), StatusCodes.Status400BadRequest);
        }
        catch (KeyNotFoundException ex)
        {
            if (context.Response.HasStarted)
                throw;

            var problem = new ProblemDetails
            {
                Title = "Resource not found",
                Detail = ex.Message,
            };

            await context.WriteProblemAsync(problem, StatusCodes.Status404NotFound);
        }
        catch (ForbiddenAccessException ex)
        {
            if (context.Response.HasStarted)
                throw;

            var problem = new ProblemDetails
            {
                Title = "Access denied",
                Detail = ex.Message,
            };

            await context.WriteProblemAsync(problem, StatusCodes.Status403Forbidden);
        }
        catch (BadHttpRequestException ex)
        {
            if (context.Response.HasStarted)
                throw;

            // Kestrel raises this for a request it refuses to read, such as a body over the size limit.
            var problem = new ProblemDetails
            {
                Title = ex.StatusCode == StatusCodes.Status413PayloadTooLarge
                    ? "The request body is too large"
                    : "The request could not be read",
            };

            await context.WriteProblemAsync(problem, ex.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while processing the request.");

            if (context.Response.HasStarted)
                throw;

            var problem = new ProblemDetails
            {
                Title = "An unexpected error occurred",
            };

            await context.WriteProblemAsync(problem, StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Builds the validation problem details describing every failure raised by a <see cref="ValidationException"/>.
    /// </summary>
    /// <param name="ex">The validation exception raised by the pipeline.</param>
    /// <returns>The validation problem details, keyed by property name.</returns>
    private static ValidationProblemDetails BuildValidationProblem(ValidationException ex)
    {
        var errors = ex.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(group => group.Key, group => group.Select(failure => failure.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Title = "Validation failed",
        };
    }
}
