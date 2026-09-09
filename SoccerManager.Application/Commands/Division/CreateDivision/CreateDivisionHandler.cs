using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Division.CreateDivision;

/// <summary>
/// Handles <see cref="CreateDivisionRequest"/> commands.
/// </summary>
public class CreateDivisionHandler : IRequestHandler<CreateDivisionRequest, int>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly ILeagueRepository _leagueRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDivisionHandler"/> class.
    /// </summary>
    /// <param name="divisionRepository">The repository used to load and persist divisions.</param>
    /// <param name="leagueRepository">The repository used to verify the owning league exists.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateDivisionHandler(IDivisionRepository divisionRepository, ILeagueRepository leagueRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _leagueRepository = leagueRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new division from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created division.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the league already has a division with the given order.</exception>
    public async Task<int> Handle(CreateDivisionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Checked explicitly so a violated foreign key does not surface as an unhandled 500.
            _ = await _leagueRepository.GetByIdAsync(request.LeagueId)
                ?? throw new KeyNotFoundException($"League {request.LeagueId} not found.");

            var existingDivisions = await _divisionRepository.GetByLeagueIdAsync(request.LeagueId);

            // Caught here so the unique (LeagueId, Order) constraint does not surface as an unhandled 500.
            if (existingDivisions.Any(d => d.Order == request.Order))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(CreateDivisionRequest.Order), $"Division with order {request.Order} already exists for league {request.LeagueId}."),
                });
            }

            var now = DateTime.UtcNow;

            var division = CreateDivisionMapper.ToDomainEntity(request, now, _currentUser.UserId);

            var id = await _divisionRepository.CreateAsync(division);

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
