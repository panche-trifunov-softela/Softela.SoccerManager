using MediatR;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Represents a request to retrieve the newest leagues available to the current user, optionally filtered by name.
/// </summary>
public sealed record GetLeaguesRequest : IRequest<GetLeaguesResponse>
{
    /// <summary>
    /// The text a league name must contain, or <see langword="null"/> for no name filter.
    /// </summary>
    public string? SearchTerm { get; init; }
}
