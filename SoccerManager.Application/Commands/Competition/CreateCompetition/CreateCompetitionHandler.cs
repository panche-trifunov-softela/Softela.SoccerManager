using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Competition.CreateCompetition;

/// <summary>
/// Handles <see cref="CreateCompetitionRequest"/> commands.
/// </summary>
public class CreateCompetitionHandler : IRequestHandler<CreateCompetitionRequest, int>
{
    private readonly ICompetitionRepository _competitionRepository;
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCompetitionHandler"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to persist competitions.</param>
    /// <param name="leagueTeamRepository">The repository used to put national teams on the league's roster.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateCompetitionHandler(ICompetitionRepository competitionRepository, ILeagueTeamRepository leagueTeamRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _competitionRepository = competitionRepository;
        _leagueTeamRepository = leagueTeamRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new competition from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created competition.</returns>
    public async Task<int> Handle(CreateCompetitionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var competition = CreateCompetitionMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _competitionRepository.CreateAsync(competition);

            // A League's national teams join its roster with its national competition, the World Cup.
            if (competition.IsNational)
            {
                await _leagueTeamRepository.CreateNationalTeamsAsync(competition.LeagueId, now, _currentUser.UserId);
            }

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
