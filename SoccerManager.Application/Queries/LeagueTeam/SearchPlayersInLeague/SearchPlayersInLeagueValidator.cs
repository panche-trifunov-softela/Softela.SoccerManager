using FluentValidation;
using SoccerManager.Application.Core.PlayerSearch;

namespace SoccerManager.Application.Queries.LeagueTeam.SearchPlayersInLeague;

/// <summary>
/// Validates <see cref="SearchPlayersInLeagueRequest"/> instances with the shared player search rules and a league team
/// identifier.
/// </summary>
public sealed class SearchPlayersInLeagueValidator : PlayerSearchFiltersValidator<SearchPlayersInLeagueRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchPlayersInLeagueValidator"/> class.
    /// </summary>
    public SearchPlayersInLeagueValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
