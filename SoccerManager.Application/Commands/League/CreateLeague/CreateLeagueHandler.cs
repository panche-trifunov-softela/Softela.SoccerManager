using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Handles <see cref="CreateLeagueRequest"/> commands.
/// </summary>
public class CreateLeagueHandler : IRequestHandler<CreateLeagueRequest, int>
{
    private readonly ILeagueRepository _leagueRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueHandler"/> class.
    /// </summary>
    /// <param name="leagueRepository">The repository used to persist leagues.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateLeagueHandler(ILeagueRepository leagueRepository, ICurrentUser currentUser)
    {
        _leagueRepository = leagueRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Creates a new league from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created league.</returns>
    public async Task<int> Handle(CreateLeagueRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var league = CreateLeagueMapper.ToDomainEntity(request, now, _currentUser.UserId);

        return await _leagueRepository.CreateAsync(league);
    }
}
