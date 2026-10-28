using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;

/// <summary>
/// Handles <see cref="CreateMatchTeamTacticRequest"/> commands.
/// </summary>
public class CreateMatchTeamTacticHandler : IRequestHandler<CreateMatchTeamTacticRequest, int>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchTeamTacticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to persist match team tactics.</param>
    /// <param name="clubAccess">The check that the caller may change the club's tactics for the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchTeamTacticHandler(IMatchTeamTacticRepository matchTeamTacticRepository, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match team tactic from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match team tactic.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when a non-operator caller's club does not play in the match or the match has already started.</exception>
    public async Task<int> Handle(CreateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var matchTeamTactic = CreateMatchTeamTacticMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            var id = await _matchTeamTacticRepository.CreateAsync(matchTeamTactic);

            await _unitOfWork.CommitAsync(cancellationToken);

            return id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
