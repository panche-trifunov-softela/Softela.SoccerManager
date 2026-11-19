using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.AnswerLeagueTeamPlayerLoanBid;

/// <summary>
/// Handles <see cref="AnswerLeagueTeamPlayerLoanBidRequest"/> commands.
/// </summary>
public class AnswerLeagueTeamPlayerLoanBidHandler : IRequestHandler<AnswerLeagueTeamPlayerLoanBidRequest, bool>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnswerLeagueTeamPlayerLoanBidHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to record the answer on the pending loan.</param>
    /// <param name="clubAccess">The check that the caller may answer for the club that holds the player.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public AnswerLeagueTeamPlayerLoanBidHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Answers a pending loan bid on a player of the given club. An accepted bid only records the agreement, so the player
    /// stays, and it rejects every other pending bid on him; a later Generator step completes accepted deals. All of it runs
    /// in one transaction.
    /// </summary>
    /// <param name="request">The answer request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the answer is recorded.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the current manager of the club that holds the player.</exception>
    /// <exception cref="ValidationException">Thrown when the bid is no longer available: it does not exist, is no longer pending, is not a bid on this player at this club, or an accept is refused because the player already has an agreed deal.</exception>
    public async Task<bool> Handle(AnswerLeagueTeamPlayerLoanBidRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

            var now = DateTime.UtcNow;

            var answered = await _leagueTeamPlayerLoanRepository.AnswerBidAsync(request.BidId, request.LeagueTeamId, request.PlayerId, request.Status, now, _currentUser.UserId);

            if (!answered)
            {
                throw new ValidationException(new[] { new ValidationFailure(nameof(request.BidId), "This bid is no longer available.") });
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
}
