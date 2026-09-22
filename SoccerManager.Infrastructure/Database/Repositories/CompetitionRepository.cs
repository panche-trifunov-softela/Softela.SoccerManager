using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="Competition"/> entities using Dapper.
/// </summary>
public class CompetitionRepository : ICompetitionRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompetitionRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public CompetitionRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new competition and returns its generated identifier.
    /// </summary>
    /// <param name="competition">The competition to create.</param>
    /// <returns>The identifier assigned to the created competition.</returns>
    public async Task<int> CreateAsync(Competition competition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", competition.LeagueId, DbType.Int32);
        parameters.Add("@Name", competition.Name, DbType.String);
        parameters.Add("@LogoUrl", competition.LogoUrl, DbType.String);
        parameters.Add("@IsDomestic", competition.IsDomestic, DbType.Boolean);
        parameters.Add("@Format", (byte)competition.Format, DbType.Byte);
        parameters.Add("@MaxAgeAllowed", competition.MaxAgeAllowed, DbType.Int32);
        parameters.Add("@Order", competition.Order, DbType.Int32);
        parameters.Add("@TeamsPromoted", competition.TeamsPromoted, DbType.Int32);
        parameters.Add("@TeamsRelegated", competition.TeamsRelegated, DbType.Int32);
        parameters.Add("@TeamsInPlayoffs", competition.TeamsInPlayoffs, DbType.Int32);
        parameters.Add("@CreatedAt", competition.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", competition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", competition.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", competition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertCompetition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing competition.
    /// </summary>
    /// <param name="competition">The competition with updated values.</param>
    /// <returns>The identifier of the updated competition.</returns>
    public async Task<int> UpdateAsync(Competition competition)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", competition.Id, DbType.Int32);
        parameters.Add("@Name", competition.Name, DbType.String);
        parameters.Add("@LogoUrl", competition.LogoUrl, DbType.String);
        parameters.Add("@IsDomestic", competition.IsDomestic, DbType.Boolean);
        parameters.Add("@Format", (byte)competition.Format, DbType.Byte);
        parameters.Add("@MaxAgeAllowed", competition.MaxAgeAllowed, DbType.Int32);
        parameters.Add("@Order", competition.Order, DbType.Int32);
        parameters.Add("@TeamsPromoted", competition.TeamsPromoted, DbType.Int32);
        parameters.Add("@TeamsRelegated", competition.TeamsRelegated, DbType.Int32);
        parameters.Add("@TeamsInPlayoffs", competition.TeamsInPlayoffs, DbType.Int32);
        parameters.Add("@ModifiedAt", competition.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", competition.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateCompetition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a competition by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteCompetition",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a competition by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to retrieve.</param>
    /// <returns>The matching competition, or <see langword="null"/> when none exists.</returns>
    public async Task<Competition?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<Competition>(
            "dbo.GetCompetitionById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves every competition belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose competitions to retrieve.</param>
    /// <returns>A list of the league's competitions.</returns>
    public async Task<List<Competition>> GetByLeagueIdAsync(int leagueId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@LeagueId", leagueId, DbType.Int32);

        var competitions = await _dapperDataContext.Connection.QueryAsync<Competition>(
            "dbo.GetCompetitions",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return competitions.ToList();
    }
}
