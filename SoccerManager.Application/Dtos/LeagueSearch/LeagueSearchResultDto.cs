namespace SoccerManager.Application.Dtos.LeagueSearch;

/// <summary>
/// Represents the result of a search of one league's players and clubs: the best few of each, with how many match in all.
/// </summary>
public sealed record LeagueSearchResultDto
{
    /// <summary>
    /// The best matching players, registered at a club of the league or not.
    /// </summary>
    public required LeagueSearchPlayerGroupDto Players { get; init; }

    /// <summary>
    /// The best matching clubs of the league.
    /// </summary>
    public required LeagueSearchTeamGroupDto Teams { get; init; }
}
