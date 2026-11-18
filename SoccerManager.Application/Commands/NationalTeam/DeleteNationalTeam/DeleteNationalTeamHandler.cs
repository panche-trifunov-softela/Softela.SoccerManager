using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeam.DeleteNationalTeam;

/// <summary>
/// Handles <see cref="DeleteNationalTeamRequest"/> commands. This performs a hard delete, as the entity carries no
/// soft-delete flag. The national team is removed, with its roster rows in every league, only when no league still has
/// standings, matches, transfers or loans for it; otherwise the removal is refused and the refusal names what blocks it.
/// </summary>
public class DeleteNationalTeamHandler : IRequestHandler<DeleteNationalTeamRequest, bool>
{
    private readonly INationalTeamRepository _nationalTeamRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteNationalTeamHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamRepository">The repository used to load and delete national teams.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteNationalTeamHandler(INationalTeamRepository nationalTeamRepository, IUnitOfWork unitOfWork)
    {
        _nationalTeamRepository = nationalTeamRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the national team identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when a league still has standings, matches, transfers or loans for the national team.</exception>
    public async Task<bool> Handle(DeleteNationalTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _nationalTeamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"NationalTeam {request.Id} not found.");

            var result = await _nationalTeamRepository.DeleteAsync(request.Id);

            if (!result.Deleted)
            {
                var blockers = DescribeBlockers(result);

                // Nothing flagged means the row vanished between the load above and the delete.
                if (blockers.Count == 0)
                {
                    throw new KeyNotFoundException($"NationalTeam {request.Id} not found.");
                }

                throw new ValidationException(new[]
                {
                    new ValidationFailure("Id", $"The national team cannot be deleted while a league still has: {string.Join(", ", blockers)}."),
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
    /// Names each kind of record that still blocks the deletion.
    /// </summary>
    /// <param name="result">The result returned by the delete.</param>
    /// <returns>The blocking parts in a fixed order, or an empty list when nothing blocks.</returns>
    private static List<string> DescribeBlockers(DeleteNationalTeamResult result)
    {
        var blockers = new List<string>();

        if (result.HasStandings)
        {
            blockers.Add("standings");
        }

        if (result.HasMatches)
        {
            blockers.Add("matches");
        }

        if (result.HasTransfers)
        {
            blockers.Add("transfers or loans");
        }

        return blockers;
    }
}
