using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeamMatch.DeleteNationalTeamMatch;

/// <summary>
/// Handles <see cref="DeleteNationalTeamMatchRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteNationalTeamMatchHandler : IRequestHandler<DeleteNationalTeamMatchRequest, bool>
{
    private readonly INationalTeamMatchRepository _nationalTeamMatchRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteNationalTeamMatchHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamMatchRepository">The repository used to load and delete national team matches.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteNationalTeamMatchHandler(INationalTeamMatchRepository nationalTeamMatchRepository, IUnitOfWork unitOfWork)
    {
        _nationalTeamMatchRepository = nationalTeamMatchRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the national team match identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team match with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteNationalTeamMatchRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _nationalTeamMatchRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"National team match {request.Id} not found.");

            await _nationalTeamMatchRepository.DeleteAsync(request.Id);

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
