using FluentValidation;
using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Validates <see cref="CreateLeagueRequest"/> instances.
/// </summary>
public sealed class CreateLeagueValidator : AbstractValidator<CreateLeagueRequest>
{
    private readonly ITeamRepository _teamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueValidator"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to check the caller has a manager profile.</param>
    /// <param name="teamRepository">The repository used to check the division clubs are existing teams.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateLeagueValidator(IManagerRepository managerRepository, ITeamRepository teamRepository, ICurrentUser currentUser)
    {
        _teamRepository = teamRepository;

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // Divisions hold 10 to 20 clubs, an even number, and a league has 1 to 5 divisions. The chain stops at the first
        // failure, so the teams are looked up only for a usable list; the lookup runs outside the handler's transaction,
        // so the foreign key is the real guarantee and this only turns an unknown team into a 400.
        RuleFor(x => x.DivisionTeamIds)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("The division clubs are required.")
            .Must(ids => ids.Count % 2 == 0 && ids.Count is >= 10 and <= 100)
            .WithMessage("The division clubs must be an even number from 10 to 100.")
            .Must(ids => ids.All(id => id > 0))
            .WithMessage("Every division club must be a valid team identifier.")
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("A team can be a division club only once.")
            .MustAsync(AllTeamsExistAsync).WithMessage("Every division club must be an existing team.");

        RuleFor(x => x.DivisionSquads).IsInEnum();

        // The lookup runs outside the handler's transaction; the handler reads the profile again, and
        // FK_Leagues_Managers enforces it.
        RuleFor(x => x).MustHaveManagerProfile(managerRepository, currentUser);
    }

    /// <summary>
    /// Checks that every given identifier belongs to an existing team.
    /// </summary>
    /// <param name="ids">The identifiers of the division clubs' teams.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when every identifier is an existing team.</returns>
    private async Task<bool> AllTeamsExistAsync(IReadOnlyList<int> ids, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync();
        var teamIds = teams.Select(team => team.Id).ToHashSet();

        return ids.All(teamIds.Contains);
    }
}
