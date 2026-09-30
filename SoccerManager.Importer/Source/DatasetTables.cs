namespace SoccerManager.Importer.Source;

/// <summary>
/// The dataset tables the importer streams, and the header columns each one must contain.
/// </summary>
public static class DatasetTables
{
    /// <summary>The table names, in the order <c>Dataset.DatasetLoader</c> streams and folds them.</summary>
    public static IReadOnlyList<string> LoadOrder { get; } = new[]
    {
        "players", "clubs", "national_teams", "games", "appearances", "game_lineups", "player_valuations",
    };

    /// <summary>
    /// The required header columns for each dataset table. A streamed table whose header is missing one of its
    /// required columns fails the read and is not retried.
    /// </summary>
    public static IReadOnlyDictionary<string, IReadOnlyList<string>> RequiredColumns { get; } =
        new Dictionary<string, IReadOnlyList<string>>(StringComparer.Ordinal)
        {
            ["players"] = new[]
            {
                "player_id", "name", "last_season", "current_club_id", "current_club_domestic_competition_id",
                "date_of_birth", "position", "sub_position", "market_value_in_eur", "current_national_team_id",
                "international_caps", "image_url",
            },
            ["clubs"] = new[]
            {
                "club_id", "name", "domestic_competition_id", "stadium_name", "stadium_seats",
                "net_transfer_record", "last_season",
            },
            ["national_teams"] = new[] { "national_team_id", "name", "team_image_url" },
            ["games"] = new[]
            {
                "game_id", "competition_id", "season", "home_club_id", "away_club_id", "home_club_goals",
                "away_club_goals", "referee",
            },
            ["appearances"] = new[] { "game_id", "player_id", "player_club_id", "minutes_played", "goals", "assists" },
            ["game_lineups"] = new[] { "game_id", "player_id", "type", "position" },
            ["player_valuations"] = new[] { "player_id", "date", "market_value_in_eur" },
        };
}
