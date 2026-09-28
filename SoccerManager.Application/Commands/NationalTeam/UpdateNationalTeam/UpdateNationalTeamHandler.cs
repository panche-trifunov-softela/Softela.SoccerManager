using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;

/// <summary>
/// Handles <see cref="UpdateNationalTeamRequest"/> commands.
/// </summary>
public class UpdateNationalTeamHandler : IRequestHandler<UpdateNationalTeamRequest, bool>
{
    private readonly INationalTeamRepository _nationalTeamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNationalTeamHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamRepository">The repository used to load and persist national teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateNationalTeamHandler(INationalTeamRepository nationalTeamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _nationalTeamRepository = nationalTeamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing national team with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateNationalTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var nationalTeam = await _nationalTeamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"NationalTeam {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateNationalTeamMapper.ApplyTo(request, nationalTeam, now, _currentUser.UserId);

            await _nationalTeamRepository.UpdateAsync(nationalTeam);

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
