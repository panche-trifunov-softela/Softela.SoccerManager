using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;

/// <summary>
/// Represents the result of a <see cref="GetNationalTeamsRequest"/> query.
/// </summary>
public sealed record GetNationalTeamsResponse
{
    /// <summary>
    /// The list of all national teams.
    /// </summary>
    public required List<NationalTeamDto> Data { get; init; }
}
