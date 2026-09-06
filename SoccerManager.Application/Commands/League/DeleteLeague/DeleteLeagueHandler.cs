using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.League.DeleteLeague;

/// <summary>
/// Handles <see cref="DeleteLeagueRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueHandler : IRequestHandler<DeleteLeagueRequest, bool>
{
    private readonly ILeagueRepository _leagueRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to load and delete leagues.</param>
    public DeleteLeagueHandler(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    /// <summary>
    /// Deletes the league identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueRequest request, CancellationToken cancellationToken)
    {
        var league = await _leagueRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League {request.Id} not found.");

        await _leagueRepository.DeleteAsync(request.Id);

        return true;
    }
}
