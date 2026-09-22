using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Competition.DeleteCompetition;

/// <summary>
/// Handles <see cref="DeleteCompetitionRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteCompetitionHandler : IRequestHandler<DeleteCompetitionRequest, bool>
{
    private readonly ICompetitionRepository _competitionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCompetitionHandler"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to load and delete competitions.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteCompetitionHandler(ICompetitionRepository competitionRepository, IUnitOfWork unitOfWork)
    {
        _competitionRepository = competitionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the competition identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no competition with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteCompetitionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _competitionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Competition {request.Id} not found.");

            await _competitionRepository.DeleteAsync(request.Id);

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
