using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

/// <summary>
/// Represents the result of a <see cref="GetNationalTeamMatchesRequest"/> query.
/// </summary>
public sealed record GetNationalTeamMatchesResponse
{
    /// <summary>
    /// The national team matches of the requested season, in kickoff order.
    /// </summary>
    public required List<NationalTeamMatchDto> Data { get; init; }
}
