using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Season.CreateSeason;

/// <summary>
/// Handles <see cref="CreateSeasonRequest"/> commands.
/// </summary>
public class CreateSeasonHandler : IRequestHandler<CreateSeasonRequest, int>
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly ILeagueRepository _leagueRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSeasonHandler"/> class.
    /// </summary>
    /// <param name="seasonRepository">The repository used to load and persist seasons.</param>
    /// <param name="leagueRepository">The repository used to verify the owning league exists.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateSeasonHandler(ISeasonRepository seasonRepository, ILeagueRepository leagueRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _seasonRepository = seasonRepository;
        _leagueRepository = leagueRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new season from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created season.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the league already has a season with the given number.</exception>
    public async Task<int> Handle(CreateSeasonRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Checked explicitly so a violated foreign key does not surface as an unhandled 500.
            _ = await _leagueRepository.GetByIdAsync(request.LeagueId)
                ?? throw new KeyNotFoundException($"League {request.LeagueId} not found.");

            var existingSeasons = await _seasonRepository.GetByLeagueIdAsync(request.LeagueId);

            // Caught here so the unique (LeagueId, SeasonNumber) constraint does not surface as an unhandled 500.
            if (existingSeasons.Any(s => s.SeasonNumber == request.SeasonNumber))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(CreateSeasonRequest.SeasonNumber), $"Season {request.SeasonNumber} already exists for league {request.LeagueId}."),
                });
            }

            var now = DateTime.UtcNow;

            var season = CreateSeasonMapper.ToDomainEntity(request, now, _currentUser.UserId);

            var id = await _seasonRepository.CreateAsync(season);

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
