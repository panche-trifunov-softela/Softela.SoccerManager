using FluentValidation;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Core.Extensions;

/// <summary>
/// Validation rules shared by commands that need the caller's manager profile.
/// </summary>
public static class ManagerProfileRuleExtensions
{
    /// <summary>
    /// Requires the caller to have a manager profile; a caller without one fails under the "Manager" key.
    /// </summary>
    /// <typeparam name="T">The type of the request being validated.</typeparam>
    /// <param name="ruleBuilder">The rule for the whole request, from <c>RuleFor(x => x)</c>.</param>
    /// <param name="managerRepository">The repository used to look up the caller's manager profile.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <returns>The rule's options, for further configuration.</returns>
    public static IRuleBuilderOptions<T, T> MustHaveManagerProfile<T>(this IRuleBuilder<T, T> ruleBuilder, IManagerRepository managerRepository, ICurrentUser currentUser)
    {
        // A caller who is not yet a manager is told to create a profile; one is never created on their behalf.
        return ruleBuilder
            .MustAsync(async (_, _) => await managerRepository.GetByUserIdAsync(currentUser.UserId) is not null)
            .OverridePropertyName("Manager")
            .WithMessage("Create a manager profile first.");
    }
}
