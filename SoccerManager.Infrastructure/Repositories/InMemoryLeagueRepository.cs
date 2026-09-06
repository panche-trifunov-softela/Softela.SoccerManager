using System.Collections.Concurrent;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Infrastructure.Repositories;

/// <summary>
/// Provides a temporary in-memory <see cref="ILeagueRepository"/> stand-in that holds data only in this process's memory and is replaced once real persistence lands.
/// </summary>
// Registered as a singleton: a scoped registration would discard every stored row between requests.
public class InMemoryLeagueRepository : ILeagueRepository
{
    private readonly ConcurrentDictionary<int, League> _leagues = new();
    private int _nextId;

    /// <summary>
    /// Assigns a new identifier to the league and stores it.
    /// </summary>
    /// <param name="league">The league to store.</param>
    /// <returns>The identifier assigned to the stored league.</returns>
    public Task<int> CreateAsync(League league)
    {
        var id = Interlocked.Increment(ref _nextId);
        league.Id = id;
        _leagues[id] = league;

        return Task.FromResult(id);
    }

    /// <summary>
    /// Replaces the stored league that shares the given league's identifier.
    /// </summary>
    /// <param name="league">The league with the updated values.</param>
    /// <returns>The identifier of the updated league.</returns>
    public Task<int> UpdateAsync(League league)
    {
        _leagues[league.Id] = league;

        return Task.FromResult(league.Id);
    }

    /// <summary>
    /// Removes the league with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to remove.</param>
    /// <returns>A task that completes once the removal has been applied.</returns>
    public Task DeleteAsync(int id)
    {
        _leagues.TryRemove(id, out _);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Retrieves the league with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to retrieve.</param>
    /// <returns>The matching league, or <see langword="null"/> when none is stored.</returns>
    public Task<League?> GetByIdAsync(int id)
    {
        return Task.FromResult(_leagues.TryGetValue(id, out var league) ? league : null);
    }

    /// <summary>
    /// Retrieves every stored league.
    /// </summary>
    /// <returns>A new list containing every stored league.</returns>
    public Task<List<League>> GetAllAsync()
    {
        return Task.FromResult(_leagues.Values.ToList());
    }
}
