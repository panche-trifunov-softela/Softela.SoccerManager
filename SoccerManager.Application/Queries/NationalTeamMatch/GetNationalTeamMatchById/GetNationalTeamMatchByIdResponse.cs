using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatchById;

/// <summary>
/// Represents the result of a <see cref="GetNationalTeamMatchByIdRequest"/> query.
/// </summary>
public sealed record GetNationalTeamMatchByIdResponse
{
    /// <summary>
    /// The requested national team match.
    /// </summary>
    public required NationalTeamMatchDto Data { get; init; }
}
