using SoccerManager.Application.Core.PlayerSearch;

namespace SoccerManager.Application.Queries.Player.SearchPlayers;

/// <summary>
/// Validates <see cref="SearchPlayersRequest"/> instances with the shared player search rules.
/// </summary>
public sealed class SearchPlayersValidator : PlayerSearchFiltersValidator<SearchPlayersRequest>
{
}
