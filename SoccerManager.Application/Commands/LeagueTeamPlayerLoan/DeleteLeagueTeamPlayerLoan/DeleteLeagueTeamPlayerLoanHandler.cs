using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.DeleteLeagueTeamPlayerLoan;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerLoanRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueTeamPlayerLoanHandler : IRequestHandler<DeleteLeagueTeamPlayerLoanRequest, bool>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerLoanHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to load and delete loans.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerLoanHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the loan identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no loan with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerLoanRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerLoanRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player loan {request.Id} not found.");

            await _leagueTeamPlayerLoanRepository.DeleteAsync(request.Id);

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
