using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayerSquadNumber;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamPlayerSquadNumberRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamPlayerSquadNumberHandler : IRequestHandler<UpdateLeagueTeamPlayerSquadNumberRequest, bool>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerSquadNumberHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load and persist league team players.</param>
    /// <param name="clubAccess">The check that the caller may change the player's club.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamPlayerSquadNumberHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Changes the squad number of an existing league team player.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team player with the given identifier exists.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="ValidationException">Thrown when another player in the club already has the squad number.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamPlayerSquadNumberRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player {request.Id} not found.");

            await _clubAccess.EnsureCanManageClubAsync(leagueTeamPlayer.LeagueTeamId);

            // The row itself does not count: keeping its own number is not a clash.
            var squad = await _leagueTeamPlayerRepository.GetByLeagueTeamIdAsync(leagueTeamPlayer.LeagueTeamId);

            if (squad.Any(member => member.Id != leagueTeamPlayer.Id && member.SquadNumber == request.SquadNumber))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(request.SquadNumber), $"Squad number {request.SquadNumber} is already taken in this club."),
                });
            }

            var now = DateTime.UtcNow;

            UpdateLeagueTeamPlayerSquadNumberMapper.ApplyTo(request, leagueTeamPlayer, now, _currentUser.UserId);

            await _leagueTeamPlayerRepository.UpdateAsync(leagueTeamPlayer);

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
