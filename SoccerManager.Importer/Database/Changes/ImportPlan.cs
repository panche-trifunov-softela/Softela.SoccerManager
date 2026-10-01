namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// The full plan for one import run: every table's planned writes, in write order, and the id lookups the writes
/// resolve their links through.
/// </summary>
/// <param name="Entities">The seven tables' plans, in write order: Stadiums, Positions, Referees, Teams, National teams, Players, Player positions.</param>
/// <param name="Ids">The ids seeded from matched existing rows, filled in further as this run's writes complete.</param>
public sealed record ImportPlan(IReadOnlyList<EntityPlan> Entities, ImportIds Ids);
