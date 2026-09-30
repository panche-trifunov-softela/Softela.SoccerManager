using SoccerManager.Importer.Dataset;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// One player's raw inputs to <see cref="RatingCalculator"/>, already resolved by <see cref="ImportModelBuilder"/>
/// from the dataset: the value, date of birth and club a player ends up importing with, rather than the dataset's
/// own raw source fields.
/// </summary>
/// <param name="TransfermarktId">The Transfermarkt player id.</param>
/// <param name="Group">The position group the rating is computed relative to.</param>
/// <param name="Value">The player's resolved value in EUR.</param>
/// <param name="DateOfBirth">The player's resolved date of birth.</param>
/// <param name="ClubTransfermarktId">The Transfermarkt id of the player's resolved current club, or <see langword="null"/> when it has none or that club was not imported.</param>
/// <param name="InternationalCaps">The player's international caps.</param>
/// <param name="SeasonStats">The player's domestic league appearance totals for each window season they played in.</param>
/// <param name="UefaMinutes">The player's UEFA minutes played, combined across the window.</param>
/// <param name="UefaGoals">The player's UEFA goals scored, combined across the window.</param>
/// <param name="UefaAssists">The player's UEFA assists recorded, combined across the window.</param>
public sealed record RatingPlayerInput(
    int TransfermarktId,
    PositionGroup Group,
    decimal Value,
    DateOnly DateOfBirth,
    int? ClubTransfermarktId,
    int InternationalCaps,
    IReadOnlyList<PlayerSeasonStats> SeasonStats,
    int UefaMinutes,
    int UefaGoals,
    int UefaAssists);
