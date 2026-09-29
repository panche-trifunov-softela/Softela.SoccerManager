namespace SoccerManager.Application.Commands.Player.UpdatePlayer;

/// <summary>
/// Applies <see cref="UpdatePlayerRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdatePlayerMapper
{
    /// <summary>
    /// Applies the request values to the given player, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new player values.</param>
    /// <param name="player">The loaded player entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdatePlayerRequest request, SoccerManager.Domain.Entities.Player player, DateTime now, Guid userId)
    {
        player.Name = request.Name;
        player.DateOfBirth = request.DateOfBirth;
        player.Rating = request.Rating;
        player.Value = request.Value;
        player.Wage = request.Wage;
        player.ImageUrl = request.ImageUrl;
        player.NationalTeamId = request.NationalTeamId;
        player.TeamId = request.TeamId;
        player.TransfermarktId = request.TransfermarktId;
        player.ModifiedAt = now;
        player.ModifiedBy = userId;
    }
}
