using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Competition.UpdateCompetition;

/// <summary>
/// Handles <see cref="UpdateCompetitionRequest"/> commands.
/// </summary>
public class UpdateCompetitionHandler : IRequestHandler<UpdateCompetitionRequest, bool>
{
    private readonly ICompetitionRepository _competitionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCompetitionHandler"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to load and persist competitions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateCompetitionHandler(ICompetitionRepository competitionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _competitionRepository = competitionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing competition with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no competition with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the competition is national and the request gives it an Order other than 0 or makes it domestic.</exception>
    public async Task<bool> Handle(UpdateCompetitionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var competition = await _competitionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Competition {request.Id} not found.");

            // Mirrors CK_Competitions_NationalIsCup for a competition created national, so a breach is a 400 rather than a
            // database error.
            if (competition.IsNational)
            {
                var failures = new List<ValidationFailure>();

                if (request.Order != 0)
                {
                    failures.Add(new ValidationFailure("Order", "A national competition is a cup, so its Order is 0."));
                }

                if (request.IsDomestic)
                {
                    failures.Add(new ValidationFailure("IsDomestic", "A national competition is not domestic."));
                }

                if (failures.Count > 0)
                {
                    throw new ValidationException(failures);
                }
            }

            var now = DateTime.UtcNow;

            UpdateCompetitionMapper.ApplyTo(request, competition, now, _currentUser.UserId);

            await _competitionRepository.UpdateAsync(competition);

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
