using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Builds the full import plan by running each table's planner, in write order, over the import model and the
/// database snapshot it is compared against.
/// </summary>
public sealed class ImportPlanner
{
    /// <summary>
    /// Plans every table's writes, in write order: Stadiums, Positions, Referees, Teams, National teams, Players,
    /// Player positions.
    /// </summary>
    /// <param name="model">The import model built from the dataset.</param>
    /// <param name="snapshot">The database snapshot the model is compared against.</param>
    /// <returns>The full import plan.</returns>
    public ImportPlan Plan(ImportModel model, DatabaseSnapshot snapshot)
    {
        var ids = new ImportIds();

        var stadiumPlan = StadiumChangePlanner.Plan(model.Stadiums, snapshot.Stadiums, ids);
        var positionPlan = PositionChangePlanner.Plan(model.Positions, snapshot.Positions, ids);
        var refereePlan = RefereeChangePlanner.Plan(model.Referees, snapshot.Referees);
        var teamPlan = TeamChangePlanner.Plan(model.Teams, snapshot.Teams, model.Stadiums, snapshot.Stadiums, ids);
        var nationalTeamPlan = NationalTeamChangePlanner.Plan(model.NationalTeams, snapshot.NationalTeams, ids);
        var playerPlan = PlayerChangePlanner.Plan(model.Players, snapshot.Players, model.Teams, snapshot.Teams, model.NationalTeams, snapshot.NationalTeams, ids);
        var playerPositionPlan = PlayerPositionChangePlanner.Plan(model.Players, snapshot.Players, snapshot.Positions, snapshot.PlayerPositionsByPlayerId);

        var entities = new[] { stadiumPlan, positionPlan, refereePlan, teamPlan, nationalTeamPlan, playerPlan, playerPositionPlan };

        return new ImportPlan(entities, ids);
    }
}
