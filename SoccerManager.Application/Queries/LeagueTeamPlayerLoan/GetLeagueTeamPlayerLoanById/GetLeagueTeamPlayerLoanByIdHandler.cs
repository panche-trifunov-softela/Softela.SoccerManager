using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoanById;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerLoanByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerLoanByIdHandler : IRequestHandler<GetLeagueTeamPlayerLoanByIdRequest, GetLeagueTeamPlayerLoanByIdResponse>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerLoanByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to load loans.</param>
    public GetLeagueTeamPlayerLoanByIdHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
    }

    /// <summary>
    /// Retrieves the loan identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested loan.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no loan with the given identifier exists.</exception>
    public async Task<GetLeagueTeamPlayerLoanByIdResponse> Handle(GetLeagueTeamPlayerLoanByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamPlayerLoan = await _leagueTeamPlayerLoanRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League team player loan {request.Id} not found.");

        return new GetLeagueTeamPlayerLoanByIdResponse { Data = GetLeagueTeamPlayerLoanByIdMapper.ToDto(leagueTeamPlayerLoan) };
    }
}
