namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayerSquadNumber;

/// <summary>
/// Applies <see cref="UpdateLeagueTeamPlayerSquadNumberRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateLeagueTeamPlayerSquadNumberMapper
{
    /// <summary>
    /// Applies the new squad number to the given league team player, leaving every other value untouched.
    /// </summary>
    /// <param name="request">The request carrying the new squad number.</param>
    /// <param name="leagueTeamPlayer">The loaded league team player entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateLeagueTeamPlayerSquadNumberRequest request, SoccerManager.Domain.Entities.LeagueTeamPlayer leagueTeamPlayer, DateTime now, Guid userId)
    {
        leagueTeamPlayer.SquadNumber = request.SquadNumber;
        leagueTeamPlayer.ModifiedAt = now;
        leagueTeamPlayer.ModifiedBy = userId;
    }
}
