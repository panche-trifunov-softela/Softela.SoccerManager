using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.DeleteMatchPlayerAbsence;

/// <summary>
/// Handles <see cref="DeleteMatchPlayerAbsenceRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchPlayerAbsenceHandler : IRequestHandler<DeleteMatchPlayerAbsenceRequest, bool>
{
    private readonly IMatchPlayerAbsenceRepository _matchPlayerAbsenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchPlayerAbsenceHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerAbsenceRepository">The repository used to load and delete match player absences.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchPlayerAbsenceHandler(IMatchPlayerAbsenceRepository matchPlayerAbsenceRepository, IUnitOfWork unitOfWork)
    {
        _matchPlayerAbsenceRepository = matchPlayerAbsenceRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match player absence identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player absence with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteMatchPlayerAbsenceRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _matchPlayerAbsenceRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match player absence {request.Id} not found.");

            await _matchPlayerAbsenceRepository.DeleteAsync(request.Id);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
