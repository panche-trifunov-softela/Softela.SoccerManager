using System.Data;
using Dapper;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Infrastructure.Database.Dapper;

namespace SoccerManager.Infrastructure.Database.Repositories;

/// <summary>
/// Provides SQL Server-backed persistence for <see cref="NationalTeam"/> entities using Dapper.
/// </summary>
public class NationalTeamRepository : INationalTeamRepository
{
    private readonly IDapperDataContext _dapperDataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalTeamRepository"/> class.
    /// </summary>
    /// <param name="dapperDataContext">The data context providing the connection and active transaction.</param>
    public NationalTeamRepository(IDapperDataContext dapperDataContext)
    {
        _dapperDataContext = dapperDataContext;
    }

    /// <summary>
    /// Persists a new national team and returns its generated identifier.
    /// </summary>
    /// <param name="nationalTeam">The national team to create.</param>
    /// <returns>The identifier assigned to the created national team.</returns>
    public async Task<int> CreateAsync(NationalTeam nationalTeam)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Name", nationalTeam.Name, DbType.String);
        parameters.Add("@StadiumId", nationalTeam.StadiumId, DbType.Int32);
        parameters.Add("@JerseyUrl", nationalTeam.JerseyUrl, DbType.String);
        parameters.Add("@LogoUrl", nationalTeam.LogoUrl, DbType.String);
        parameters.Add("@CreatedAt", nationalTeam.CreatedAt, DbType.DateTime2);
        parameters.Add("@ModifiedAt", nationalTeam.ModifiedAt, DbType.DateTime2);
        parameters.Add("@CreatedBy", nationalTeam.CreatedBy, DbType.Guid);
        parameters.Add("@ModifiedBy", nationalTeam.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.InsertNationalTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Persists changes to an existing national team.
    /// </summary>
    /// <param name="nationalTeam">The national team with updated values.</param>
    /// <returns>The identifier of the updated national team.</returns>
    public async Task<int> UpdateAsync(NationalTeam nationalTeam)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", nationalTeam.Id, DbType.Int32);
        parameters.Add("@Name", nationalTeam.Name, DbType.String);
        parameters.Add("@StadiumId", nationalTeam.StadiumId, DbType.Int32);
        parameters.Add("@JerseyUrl", nationalTeam.JerseyUrl, DbType.String);
        parameters.Add("@LogoUrl", nationalTeam.LogoUrl, DbType.String);
        parameters.Add("@ModifiedAt", nationalTeam.ModifiedAt, DbType.DateTime2);
        parameters.Add("@ModifiedBy", nationalTeam.ModifiedBy, DbType.Guid);

        return await _dapperDataContext.Connection.QuerySingleAsync<int>(
            "dbo.UpdateNationalTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes a national team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        await _dapperDataContext.Connection.ExecuteAsync(
            "dbo.DeleteNationalTeam",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a national team by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to retrieve.</param>
    /// <returns>The matching national team, or <see langword="null"/> when none exists.</returns>
    public async Task<NationalTeam?> GetByIdAsync(int id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id, DbType.Int32);

        return await _dapperDataContext.Connection.QueryFirstOrDefaultAsync<NationalTeam>(
            "dbo.GetNationalTeamById",
            parameters,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all national teams.
    /// </summary>
    /// <returns>A list of every national team.</returns>
    public async Task<List<NationalTeam>> GetAllAsync()
    {
        var nationalTeams = await _dapperDataContext.Connection.QueryAsync<NationalTeam>(
            "dbo.GetNationalTeams",
            param: null,
            transaction: _dapperDataContext.Transaction,
            commandType: CommandType.StoredProcedure).ConfigureAwait(false);

        return nationalTeams.ToList();
    }
}
