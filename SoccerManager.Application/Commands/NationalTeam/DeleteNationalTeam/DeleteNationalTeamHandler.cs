using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeam.DeleteNationalTeam;

/// <summary>
/// Handles <see cref="DeleteNationalTeamRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
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
    public async Task<bool> Handle(DeleteNationalTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _nationalTeamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"NationalTeam {request.Id} not found.");

            await _nationalTeamRepository.DeleteAsync(request.Id);

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
