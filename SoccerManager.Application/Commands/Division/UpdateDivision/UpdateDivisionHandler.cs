using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Division.UpdateDivision;

/// <summary>
/// Handles <see cref="UpdateDivisionRequest"/> commands.
/// </summary>
public class UpdateDivisionHandler : IRequestHandler<UpdateDivisionRequest, bool>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDivisionHandler"/> class.
    /// </summary>
    /// <param name="divisionRepository">The repository used to load and persist divisions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateDivisionHandler(IDivisionRepository divisionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing division with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no division with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the owning league already has a different division with the given order.</exception>
    public async Task<bool> Handle(UpdateDivisionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var division = await _divisionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Division {request.Id} not found.");

            var leagueDivisions = await _divisionRepository.GetByLeagueIdAsync(division.LeagueId);

            // Caught here so the unique (LeagueId, Order) constraint does not surface as an unhandled 500.
            if (leagueDivisions.Any(d => d.Order == request.Order && d.Id != request.Id))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(UpdateDivisionRequest.Order), $"Division with order {request.Order} already exists for league {division.LeagueId}."),
                });
            }

            var now = DateTime.UtcNow;

            UpdateDivisionMapper.ApplyTo(request, division, now, _currentUser.UserId);

            await _divisionRepository.UpdateAsync(division);

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
