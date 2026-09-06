using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.League.GetLeagueById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueByIdResponse
{
    /// <summary>
    /// The requested league.
    /// </summary>
    public required LeagueDto Data { get; init; }
}
