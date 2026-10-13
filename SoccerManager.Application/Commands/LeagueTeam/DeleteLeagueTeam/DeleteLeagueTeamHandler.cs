using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag. The roster entry is removed only when nothing is attached to
/// the club in that league: while it still has a squad, manager appointments, applications, standings
/// or matches the removal is refused, and the refusal names what blocks it.
/// </summary>
public class DeleteLeagueTeamHandler : IRequestHandler<DeleteLeagueTeamRequest, bool>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load and delete league teams.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamHandler(ILeagueTeamRepository leagueTeamRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the club still has a squad, appointments, applications, standings or matches in the league.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeam {request.Id} not found.");

            var result = await _leagueTeamRepository.DeleteAsync(request.Id);

            if (!result.Deleted)
            {
                var blockers = DescribeBlockers(result);

                // Nothing flagged means the row vanished between the load above and the delete.
                if (blockers.Count == 0)
                {
                    throw new KeyNotFoundException($"LeagueTeam {request.Id} not found.");
                }

                throw new ValidationException(new[]
                {
                    new ValidationFailure("Id", $"The team cannot be removed from this league while it still has: {string.Join(", ", blockers)}."),
                });
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Names each kind of record that still blocks the removal.
    /// </summary>
    /// <param name="result">The result returned by the delete.</param>
    /// <returns>The blocking parts in a fixed order, or an empty list when nothing blocks.</returns>
    private static List<string> DescribeBlockers(DeleteLeagueTeamResult result)
    {
        var blockers = new List<string>();

        if (result.HasSquad)
        {
            blockers.Add("a squad");
        }

        if (result.HasAppointments)
        {
            blockers.Add("manager appointments");
        }

        if (result.HasApplications)
        {
            blockers.Add("applications");
        }

        if (result.HasStandings)
        {
            blockers.Add("standings");
        }

        if (result.HasMatches)
        {
            blockers.Add("matches");
        }

        return blockers;
    }
}
