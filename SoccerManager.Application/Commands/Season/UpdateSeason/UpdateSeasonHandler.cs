using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Season.UpdateSeason;

/// <summary>
/// Handles <see cref="UpdateSeasonRequest"/> commands.
/// </summary>
public class UpdateSeasonHandler : IRequestHandler<UpdateSeasonRequest, bool>
{
    private readonly ISeasonRepository _seasonRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSeasonHandler"/> class.
    /// </summary>
    /// <param name="seasonRepository">The repository used to load and persist seasons.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateSeasonHandler(ISeasonRepository seasonRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _seasonRepository = seasonRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing season with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no season with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the owning league already has a different season with the given number.</exception>
    public async Task<bool> Handle(UpdateSeasonRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var season = await _seasonRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Season {request.Id} not found.");

            var leagueSeasons = await _seasonRepository.GetByLeagueIdAsync(season.LeagueId);

            // Caught here so the unique (LeagueId, SeasonNumber) constraint does not surface as an unhandled 500.
            if (leagueSeasons.Any(s => s.SeasonNumber == request.SeasonNumber && s.Id != request.Id))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(UpdateSeasonRequest.SeasonNumber), $"Season {request.SeasonNumber} already exists for league {season.LeagueId}."),
                });
            }

            var now = DateTime.UtcNow;

            UpdateSeasonMapper.ApplyTo(request, season, now, _currentUser.UserId);

            await _seasonRepository.UpdateAsync(season);

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
