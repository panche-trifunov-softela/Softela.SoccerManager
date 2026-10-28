using Microsoft.AspNetCore.Mvc;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Writes problem-details responses in the format every API error uses.
/// </summary>
public static class ProblemResponseExtensions
{
    /// <summary>
    /// Writes the given problem details to the response with the given status code.
    /// </summary>
    /// <typeparam name="TProblem">The concrete problem-details type being written.</typeparam>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="problem">The problem details to write.</param>
    /// <param name="statusCode">The HTTP status code to return.</param>
    /// <returns>A task that completes once the response has been written.</returns>
    // Generic on the concrete type: serializing through the ProblemDetails base would drop
    // ValidationProblemDetails.Errors, since System.Text.Json writes the declared type's properties.
    public static async Task WriteProblemAsync<TProblem>(this HttpContext context, TProblem problem, int statusCode)
        where TProblem : ProblemDetails
    {
        problem.Status = statusCode;
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problem);
    }
}
