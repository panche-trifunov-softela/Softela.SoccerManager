using MediatR;

namespace SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

/// <summary>
/// Represents a request to retrieve every national team match of a season.
/// </summary>
public sealed record GetNationalTeamMatchesRequest : IRequest<GetNationalTeamMatchesResponse>
{
    /// <summary>
    /// The identifier of the season whose national team matches are being requested.
    /// </summary>
    public int SeasonId { get; init; }
}
