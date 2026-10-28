using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SoccerManager.API.RateLimiting;
using SoccerManager.API.Security;

namespace SoccerManager.API.Extensions;

/// <summary>
/// Registers the API's rate limits and the response a caller gets when it goes over one.
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Adds a per-caller limit on every request, plus the tighter policies endpoints opt into with
    /// <see cref="EnableRateLimitingAttribute"/>.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same service collection, for chaining.</returns>
    public static IServiceCollection AddApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = WriteRejectionAsync;

            // A caller without a valid token, /health included, gets the lower limit, counted per client IP.
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                PerMinute(CallerKey(context), context.User.GetSubject() is null
                    ? RateLimitPolicies.AnonymousPerMinute
                    : RateLimitPolicies.SignedInPerMinute));

            options.AddPolicy(RateLimitPolicies.Search, context =>
                PerMinute(CallerKey(context), RateLimitPolicies.SearchPerMinute));

            options.AddPolicy(RateLimitPolicies.FullTable, context =>
                PerMinute(CallerKey(context), RateLimitPolicies.FullTablePerMinute));
        });

        return services;
    }

    /// <summary>
    /// Gets the key a caller is counted under: its Keycloak user id when signed in, otherwise its client IP.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>The partition key for the caller.</returns>
    private static string CallerKey(HttpContext context)
    {
        // Behind Azure Container Apps the client IP comes from X-Forwarded-For, which the host reads because
        // ASPNETCORE_FORWARDEDHEADERS_ENABLED is set in infra/api.bicep.
        return context.User.GetSubject() is { } subject
            ? $"user:{subject}"
            : $"ip:{context.Connection.RemoteIpAddress}";
    }

    /// <summary>
    /// Creates a fixed one-minute window for the given key that allows the given number of requests and queues none.
    /// </summary>
    /// <param name="key">The partition key the window counts requests under.</param>
    /// <param name="permitLimit">The number of requests allowed per minute.</param>
    /// <returns>The rate-limit partition for the key.</returns>
    private static RateLimitPartition<string> PerMinute(string key, int permitLimit)
    {
        return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
        });
    }

    /// <summary>
    /// Answers a rejected request with a problem-details body and, when the limiter knows it, how long to wait.
    /// </summary>
    /// <param name="context">The rejected request and the lease that refused it.</param>
    /// <param name="cancellationToken">The token that is cancelled when the request is aborted.</param>
    /// <returns>A task that completes once the response has been written.</returns>
    private static async ValueTask WriteRejectionAsync(OnRejectedContext context, CancellationToken cancellationToken)
    {
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            context.HttpContext.Response.Headers.RetryAfter = Math.Ceiling(retryAfter.TotalSeconds).ToString(CultureInfo.InvariantCulture);

        var problem = new ProblemDetails
        {
            Title = "Too many requests. Please wait a minute and try again.",
        };

        await context.HttpContext.WriteProblemAsync(problem, StatusCodes.Status429TooManyRequests);
    }
}
