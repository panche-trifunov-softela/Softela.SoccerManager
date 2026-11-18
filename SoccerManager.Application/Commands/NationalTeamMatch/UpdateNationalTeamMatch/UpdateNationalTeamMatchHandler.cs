using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.NationalTeamMatch.UpdateNationalTeamMatch;

/// <summary>
/// Handles <see cref="UpdateNationalTeamMatchRequest"/> commands.
/// </summary>
public class UpdateNationalTeamMatchHandler : IRequestHandler<UpdateNationalTeamMatchRequest, bool>
{
    private readonly INationalTeamMatchRepository _nationalTeamMatchRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNationalTeamMatchHandler"/> class.
    /// </summary>
    /// <param name="nationalTeamMatchRepository">The repository used to load and persist national team matches.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateNationalTeamMatchHandler(INationalTeamMatchRepository nationalTeamMatchRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _nationalTeamMatchRepository = nationalTeamMatchRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing national team match with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no national team match with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateNationalTeamMatchRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var nationalTeamMatch = await _nationalTeamMatchRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"National team match {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateNationalTeamMatchMapper.ApplyTo(request, nationalTeamMatch, now, _currentUser.UserId);

            await _nationalTeamMatchRepository.UpdateAsync(nationalTeamMatch);

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
