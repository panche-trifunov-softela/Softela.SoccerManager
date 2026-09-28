using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeamById;

/// <summary>
/// Represents the result of a <see cref="GetNationalTeamByIdRequest"/> query.
/// </summary>
public sealed record GetNationalTeamByIdResponse
{
    /// <summary>
    /// The requested national team.
    /// </summary>
    public required NationalTeamDto Data { get; init; }
}
