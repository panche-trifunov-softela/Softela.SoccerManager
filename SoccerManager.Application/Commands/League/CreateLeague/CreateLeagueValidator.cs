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
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueValidator"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to check the caller has a manager profile.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateLeagueValidator(IManagerRepository managerRepository, ICurrentUser currentUser)
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // The lookup runs outside the handler's transaction; the handler reads the profile again, and
        // FK_Leagues_Managers enforces it.
        RuleFor(x => x).MustHaveManagerProfile(managerRepository, currentUser);
    }
}
